import { createContext, useState, useContext, ReactNode, useEffect, useRef } from 'react';
import { UsersService } from '../../services/UsersService';
import {jwtDecode} from 'jwt-decode';

interface DecodedToken {
    exp: number; // expiration time
    userId: string; // userId
    sub: string; // username (new field)
}

interface AuthContextType {
    token: string | null;
    currentUserId: string | null;
    currentUsername: string | null; 
    login: (username: string, password: string) => Promise<void>;
    logout: () => void;
    isAuthenticated: boolean;
    register: (username: string, password: string) => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [token, setToken] = useState<string | null>(null);
    const [userId, setUserId] = useState<string | null>(null);
    const [username, setUsername] = useState<string | null>(null);
    const renewalTimerRef = useRef<ReturnType<typeof setTimeout> | null>(null);

    useEffect(() => {
        const usersService = UsersService.getInstance();
        const storedToken = usersService.getToken();
        if (storedToken) {
            setToken(storedToken);
            const decoded: DecodedToken = jwtDecode(storedToken);
            setUserId(decoded.userId);
            scheduleTokenRenewal(decoded.exp);
        }
    }, []);

    const login = async (username: string, password: string) => {
        const usersService = UsersService.getInstance();
        const { token } = await usersService.loginUser({ username, password });
        setToken(token);

        const decoded: DecodedToken = jwtDecode(token);
        setUserId(decoded.userId);
        setUsername(decoded.sub);
        scheduleTokenRenewal(decoded.exp);
    };

    const logout = () => {
        const usersService = UsersService.getInstance();
        usersService.clearToken();
        setToken(null);
        setUserId(null);
        setUsername(null);

        if (renewalTimerRef.current) {
            clearTimeout(renewalTimerRef.current);
            renewalTimerRef.current = null;
        }
    };

    const register = async (username: string, password: string) => {
        const usersService = UsersService.getInstance();
        await usersService.registerUser({ username, password });
    };

    const scheduleTokenRenewal = (exp: number) => {
        const currentTime = Math.floor(Date.now() / 1000);
        const timeUntilExpiration = exp - currentTime;
        const renewalTime = timeUntilExpiration - 300;

        if (renewalTimerRef.current) {
            clearTimeout(renewalTimerRef.current);
        }

        if (renewalTime > 0) {
            renewalTimerRef.current = setTimeout(async () => {
                try {
                    const usersService = UsersService.getInstance();
                    const { token: newToken } = await usersService.refreshToken();
                    setToken(newToken);

                    const decoded: DecodedToken = jwtDecode(newToken);
                    setUserId(decoded.userId);
                    setUsername(decoded.sub);
                    scheduleTokenRenewal(decoded.exp);
                } catch (error) {
                    console.error('Failed to renew token:', error);
                    logout();
                }
            }, renewalTime * 1000);
        }
    };

    const isAuthenticated = !!token;

    return (
        <AuthContext.Provider value={{ token, login, logout, isAuthenticated, register, currentUserId: userId, currentUsername: username,  }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};