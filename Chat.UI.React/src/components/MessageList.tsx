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

interface Props {
  messages: Message[];
  onDeleteMessage: (id: string) => void; // Callback to handle message deletion
}

const MessageList: React.FC<Props> = ({ messages, onDeleteMessage }) => {
  const bottomRef = useRef<HTMLDivElement | null>(null);
  const { currentUserId } = useAuth();
  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: 'smooth' });
    console.log('currentUserId: ' + currentUserId);
  }, [messages]);

  const handleDelete = async (id: string) => {
    try {
      const messagesService = MessagesService.getInstance();
      await messagesService.deleteMessage(id); // Call the deleteMessage function
      onDeleteMessage(id); // Notify parent component to update the message list
    } catch (error) {
      console.error('Failed to delete message:', error);
    }
  };

  return (
    <div className="message-list">
      {messages.map((msg) => (
        console.log('Message:', msg), // Debugging
        <div key={msg.id} className="message-item">
        {(() => {
          switch (msg.type) {
            case 'text':
              return (
                <TextMessage
                  content={msg.content}
                  isOwner={msg.userId === currentUserId}
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
        // <div key={msg.id} className="message-item">
        //   {(() => {
        //     switch (msg.type) {
        //       case 'text':
        //         return <TextMessage content={msg.content} isMine={msg.userId == '1'} onDelete={() => handleDelete(msg.id) />;
        //       case 'image':
        //         return <ImageMessage url={msg.content} />;
        //       case 'chart':
        //         return <ChartMessage data={msg.content} metadata={msg.metadata} />;
        //       case 'table':
        //         return <TableMessage markdown={msg.content} />;
        //       default:
        //         return null;
        //     }
        //   })()}
        //   {/* <button
        //     className="delete-button"
        //     onClick={() => handleDelete(msg.id)}
        //     aria-label="Delete message"
        //   >
        //     <FaTrash />
        //   </button> */}
         
      
        // </div>
      ))}
      <div ref={bottomRef} />
    </div>
  );
};

export default MessageList;
