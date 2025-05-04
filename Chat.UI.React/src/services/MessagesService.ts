import axios from 'axios';
import { Message } from '../types/Message';
// Removed unnecessary import as AxiosInstance is directly accessible from axios

export class MessagesService {
    private static instance: MessagesService;
    private client: Axios.AxiosInstance;

    private constructor(baseURL: string) {
        this.client = axios.create({
            baseURL,
            headers: {
                'Content-Type': 'application/json',
            },
        });
    }

    public static getInstance(baseURL: string): MessagesService {
        if (!MessagesService.instance) {
            MessagesService.instance = new MessagesService(baseURL);
        }
        return MessagesService.instance;
    }

    async fetchMessages(): Promise<Message[]> {
        try {
            const response = await this.client.get('/messages');
            return response.data  as Message[]; // Assuming the API returns { data: [...] }
        } catch (error: any) {
            throw new Error(error.response?.data?.message || 'Failed to fetch messages');
        }
    }

    async addMessage(message: Message): Promise<Message> {
        try {
            const response = await this.client.post('/messages', message);
            return response.data as Message; // Assuming the API returns { data: {...} }
        } catch (error: any) {
            throw new Error(error.response?.data?.message || 'Failed to add message');
        }
    }

    async deleteMessage(id: string): Promise<void> {
        try {
            await this.client.delete(`/data/${id}`);
        } catch (error: any) {
            throw new Error(error.response?.data?.message || 'Failed to delete message');
        }
    }
}