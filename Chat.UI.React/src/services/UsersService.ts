import axios from 'axios';
import { LoginInfo, AuthResult } from '../types/User';

export class UsersService {
    private static instance: UsersService;
    private client: Axios.AxiosInstance;
    private token: string | null = null;

    private constructor() {
        const baseURL: string = import.meta.env.VITE_API_BASE_URL;
        this.client = axios.create({
            baseURL,
            headers: {
                'Content-Type': 'application/json',
            },
        });
    }

    public static getInstance(): UsersService {
        if (!UsersService.instance) {
            UsersService.instance = new UsersService();
        }
        return UsersService.instance;
    }

    setToken(token: string): void {
        this.token = token;
        localStorage.setItem('authToken', token); // Persist token to localStorage
        console.log('Token set and persisted:', token); // Debugging
    }
    
    getToken(): string | null {
        if (!this.token) {
            this.token = localStorage.getItem('authToken'); // Retrieve token from localStorage
        }
        return this.token;
    }
    clearToken(): void {
        this.token = null;
        localStorage.removeItem('authToken'); // Remove token from localStorage
    }
    async registerUser(registerDetails: LoginInfo): Promise<{ message: string }> {
        try {
            const response = await this.client.post('/users/register', registerDetails);
            return response.data as { message: string };
        } catch (error: any) {
            throw new Error(error.response?.data?.message || 'Failed to register user');
        }
    }

    async loginUser(loginInfo: LoginInfo): Promise<AuthResult> {
        try {
            const response = await this.client.post('/users/login', loginInfo);
            const authResult = response.data as AuthResult;           
            this.setToken(authResult.token); // Store the token after login
            return authResult;
        } catch (error: any) {
            throw new Error(error.response?.data?.message || 'Failed to login user');
        }
    }
    async refreshToken(): Promise<AuthResult> {
        try {
            const token = this.getToken(); // Retrieve the current token
            if (!token) {
                throw new Error('No token available for refresh');
            }
    
            const response = await this.client.get('/users/refresh', {
                headers: {
                    Authorization: `Bearer ${token}`, // Add the token to the Authorization header
                },
            });
    
            const authResult = response.data as AuthResult;
            this.setToken(authResult.token); // Store the new token after refresh
            return authResult;
        } catch (error: any) {
            throw new Error(error.response?.data?.message || 'Failed to refresh the token.');
        }
    }
}