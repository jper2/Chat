// Defines the types of messages supported in the app.
export type MessageType = 'text' | 'image' | 'chart' | 'table';

export interface Message {
  id: string;               // Unique identifier (e.g., UUID or timestamp-based).
  type: MessageType;        // Determines which component renders the message.
  content: string;          // The payload (Markdown, image URL, etc.).
  metadata?: {
    chartType?: 'line' | 'bar' | 'area' | 'pie'; // Add more types as needed
  };
  userId:string;
}