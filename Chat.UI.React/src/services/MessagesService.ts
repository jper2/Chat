// filepath: c:\Users\jpereira\source\repos\Chat\Chat.UI.React\src\services\MessagesService.ts
import axios from 'axios';
import { Message } from '../types/Message';
import { UsersService } from './UsersService';

export class MessagesService {
    private static instance: MessagesService;
    private client: Axios.AxiosInstance;

    private constructor() {
        const baseURL: string = import.meta.env.VITE_API_BASE_URL;
        const usersService = UsersService.getInstance();
        this.client = axios.create({
            baseURL,
            headers: {
                'Content-Type': 'application/json',
            },
        });

        // Add a request interceptor to dynamically include the token
        this.client.interceptors.request.use((config) => {
            const token = usersService.getToken();
            if (token) {
                config.headers = config.headers || {};
                config.headers['Authorization'] = `Bearer ${token}`;
            }
            return config;
        });
    }

    public static getInstance(): MessagesService {
        if (!MessagesService.instance) {
            MessagesService.instance = new MessagesService();
        }
        return MessagesService.instance;
    }

    async fetchMessages(): Promise<Message[]> {
        try {
            const response = await this.client.get('/api/messages');
            return response.data as Message[];
        } catch (error: any) {
            throw new Error(error.response?.data?.message || 'Failed to fetch messages');
        }
    }

    async addMessage(message: Message): Promise<Message> {
        try {
            const msgToSend = {
                //id: '',
                content: message.content,
                type: message.type,
                userId: '21', // Ensure this is set
                metadata: {}, // Default to an empty object if not provided
            };
            const response = await this.client.post('/api/messages', msgToSend);
            return response.data as Message;
        } catch (error: any) {
            throw new Error(error.response?.data?.message || 'Failed to add message');
        }
    }

    async deleteMessage(id: string): Promise<void> {
        try {
            await this.client.delete(`/api/messages/${id}`);
        } catch (error: any) {
            throw new Error(error.response?.data?.message || 'Failed to delete message');
        }
    }
}