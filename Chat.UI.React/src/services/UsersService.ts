import axios from 'axios';
import { LoginInfo, AuthResult } from '../types/User';

export class UsersService {
    private static instance: UsersService;
    private client: Axios.AxiosInstance;

    private constructor(baseURL: string) {
        this.client = axios.create({
            baseURL,
            headers: {
                'Content-Type': 'application/json',
            },
        });
    }

    public static getInstance(baseURL: string): UsersService {
        if (!UsersService.instance) {
            UsersService.instance = new UsersService(baseURL);
        }
        return UsersService.instance;
    }

    async registerUser(registerDetails: LoginInfo): Promise<{ message: string }> {
        try {
            const response = await this.client.post('/api/users/register', registerDetails);
            return response.data as { message: string };
        } catch (error: any) {
            throw new Error(error.response?.data?.message || 'Failed to register user');
        }
    }

    async loginUser(loginInfo: LoginInfo): Promise<AuthResult> {
        try {
            const response = await this.client.post('/api/users/login', loginInfo);
            return response.data as AuthResult;
        } catch (error: any) {
            throw new Error(error.response?.data?.message || 'Failed to login user');
        }
    }

    // async getCurrentUser(): Promise<User> {
    //     try {
    //         const response = await this.client.get('/api/users/me');
    //         return response.data as User; // Assuming the API returns the user object
    //     } catch (error: any) {
    //         throw new Error(error.response?.data?.message || 'Failed to fetch current user');
    //     }
    // }
}