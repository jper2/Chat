import { Message } from '../types/Message';
import TextMessage from './messages/TextMessage';
import ImageMessage from './messages/ImageMessage';
import ChartMessage from './messages/ChartMessage';
import TableMessage from './messages/TableMessage';
import { useEffect, useRef } from 'react';
// import { FaTrash } from 'react-icons/fa'; // Import a trash icon from react-icons
// import { Trash2 } from 'lucide-react';
import { MessagesService } from '../services/MessagesService';
import { useAuth } from './context/AuthContext';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

interface Props {
  messages: Message[];
  onDeleteMessage: (id: string) => void; // Callback to handle message deletion
}

const MessageList: React.FC<Props> = ({ messages, onDeleteMessage }) => {
  const bottomRef = useRef<HTMLDivElement | null>(null);
  const { currentUserId } = useAuth();
  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  const handleDelete = async (id: string) => {
    try {
      const messagesService = MessagesService.getInstance();
      await messagesService.deleteMessage(id); // Call the deleteMessage function
      onDeleteMessage(id); // Notify parent component to update the message list
    } catch (error) {
      toast.error('Failed to delete message. Please try again.');
    }
  };

  return (
    <div className="message-list">
      {messages.map((msg) => (
        <div key={msg.id} className="message-item">
        {(() => {
          switch (msg.type) {
            case 'text':
              return (
                <TextMessage
                  id={msg.id}
                  content={msg.content}
                  createdByCurrentUser={msg.userId === currentUserId}
                  onDelete={() => handleDelete(msg.id)}
                />
              );
            case 'image':
              return (
                <ImageMessage
                  url={msg.content}
                  onDelete={() => handleDelete(msg.id)}
                />
              );
            case 'chart':
              return (
                <ChartMessage
                  data={msg.content}
                  metadata={msg.metadata}
                  onDelete={() => handleDelete(msg.id)}
                />
              );
            case 'table':
              return (
                <TableMessage
                  markdown={msg.content}
                  onDelete={() => handleDelete(msg.id)}
                />
              );
            default:
              return null;
          }
        })()}
      </div>
      ))}
      <div ref={bottomRef} />
      <ToastContainer position="top-right" autoClose={3000} />
    </div>
  );
};

export default MessageList;
