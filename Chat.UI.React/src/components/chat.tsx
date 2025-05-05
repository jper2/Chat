import { useEffect, useState, useRef, useCallback } from 'react';
import MessageList from './MessageList';
import MessageInput from './MessageInput';
import { Message } from '../types/Message';
import { MessagesService } from '../services/MessagesService';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import * as signalR from '@microsoft/signalr';
import { mockMessages } from '../data/mockMessages';

const Chat = () => {
  const [messages, setMessages] = useState<Message[]>(mockMessages);
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const messagesService = MessagesService.getInstance();
  const baseURL = import.meta.env.VITE_API_BASE_URL;

  // Fetch messages on mount
  useEffect(() => {
    const fetchMessages = async () => {
      try {
        const fetchedMessages = await messagesService.fetchMessages();
        setMessages((prevMessages) => {
          const allMessages = [...prevMessages, ...fetchedMessages];
          const uniqueMessages = Array.from(
            new Map(allMessages.map((msg) => [msg.id, msg])).values()
          );
          return uniqueMessages;
        });
      } catch (error) {
        console.error('Failed to fetch messages:', error);
        if (!toast.isActive('fetchError')) {
          toast.error('Failed to load messages. Please try again later.', {
            toastId: 'fetchError',
          });
        }
      }
    };

    fetchMessages();
  }, []);

  // Setup SignalR
  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${baseURL}/hubs/chat`, {
        withCredentials: true, // Include credentials in the request
      })
      .withAutomaticReconnect()
      .build();

    connection
      .start()
      .then(() => {
        console.log('Connected to SignalR hub');

        // Listen for the "MessageAdded" event
        connection.on('MessageAdded', (newMessage: Message) => {
          setMessages((prev) => [...prev, newMessage]);
        });

        // Listen for the "MessageDeleted" event
        connection.on('MessageDeleted', (deletedMessageId: string) => {
          setMessages((prev) => prev.filter((msg) => msg.id !== deletedMessageId));
        });
      })
      .catch((err) => {
        console.error('SignalR connection error:', err);
        if (!toast.isActive('signalRError')) {
          toast.error('Failed to connect to the chat server.', {
            toastId: 'signalRError',
          });
        }
      });

    connectionRef.current = connection;

    return () => {
      connection.off('MessageAdded');
      connection.off('MessageDeleted');
      connection.stop();
    };
  }, [baseURL]);

  const handleSendMessage = useCallback(async (content: string) => {
    const newMessage: Message = {
      type: 'text',
      content,
      id:'',
      userId: '21', // Ensure this is set to the current user's ID
      metadata: {}, // Default to an empty object if not provided
    };

    try {
      await messagesService.addMessage(newMessage); // Message will come back via SignalR
    } catch (error) {
      toast.error('Failed to send message. Please try again.');
    }
  }, []);

  const handleDeleteMessage = useCallback((id: string) => {
    setMessages((prev) => prev.filter((msg) => msg.id !== id));
  }, []);

  return (
    <div className="min-h-screen flex flex-col items-center px-4 py-6">
      <div className="w-full max-w-2xl">
        <MessageList messages={messages} onDeleteMessage={handleDeleteMessage} />
        <div className="h-20" />
        <div className="sticky bottom-0 pt-4 z-10 shadow-t">
          <MessageInput onSend={handleSendMessage} />
        </div>
      </div>
      <ToastContainer position="bottom-right" autoClose={3000} />
    </div>
  );
};

export default Chat;
