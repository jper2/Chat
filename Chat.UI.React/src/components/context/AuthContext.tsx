import React, { createContext, useState, useContext, ReactNode, useEffect, useRef } from 'react';
import { UsersService } from '../../services/UsersService';
import {jwtDecode} from 'jwt-decode';

interface DecodedToken {
    exp: number; // expiration time
    userId: string; // userId
}

interface AuthContextType {
    token: string | null;
    currentUserId: string | null;
    login: (username: string, password: string) => Promise<void>;
    logout: () => void;
    isAuthenticated: boolean;
    register: (username: string, password: string) => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [token, setToken] = useState<string | null>(null);
    const [userId, setUserId] = useState<string | null>(null);
    const renewalTimerRef = useRef<ReturnType<typeof setTimeout> | null>(null); // Ref to store the timer ID

    // Decode the token and set the userId
    useEffect(() => {
        if (token) {
            const decoded: DecodedToken = jwtDecode(token);
            setUserId(decoded.userId);
            scheduleTokenRenewal(decoded.exp);
        }
    }, [token]);

    const login = async (username: string, password: string) => {
        const usersService = UsersService.getInstance();
        const { token } = await usersService.loginUser({ username, password });
        setToken(token);
        localStorage.setItem('authToken', token); // Persist token

        const decoded: DecodedToken = jwtDecode(token);
        setUserId(decoded.userId);
        scheduleTokenRenewal(decoded.exp);
    };

    const logout = () => {
        const usersService = UsersService.getInstance();
        usersService.clearToken();
        setToken(null);
        setUserId(null);
        localStorage.removeItem('authToken');

        // Clear the renewal timer on logout
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
        const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds
        const timeUntilExpiration = exp - currentTime;
        const renewalTime = timeUntilExpiration - 300; // Renew 5 minutes before expiration

        // Clear any existing timer before setting a new one
        if (renewalTimerRef.current) {
            clearTimeout(renewalTimerRef.current);
        }

        if (renewalTime > 0) {
            renewalTimerRef.current = setTimeout(async () => {
                try {
                    const usersService = UsersService.getInstance();
                    const { token: newToken } = await usersService.refreshToken(); // Call the refresh endpoint
                    setToken(newToken);
                    localStorage.setItem('authToken', newToken);

                    const decoded: DecodedToken = jwtDecode(newToken);
                    setUserId(decoded.userId);
                    scheduleTokenRenewal(decoded.exp); // Schedule the next renewal
                } catch (error) {
                    console.error('Failed to renew token:', error);
                    logout(); // Log out if token renewal fails
                }
            }, renewalTime * 1000); // Convert to milliseconds
        }
    };

    const isAuthenticated = !!token;

    return (
        <AuthContext.Provider value={{ token, login, logout, isAuthenticated, register, currentUserId: userId }}>
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