import { useEffect, useState } from 'react';
import MessageList from './MessageList';
import MessageInput from './MessageInput';
import { mockMessages } from '../data/mockMessages';
import { Message } from '../types/Message';
import { MessagesService } from '../services/MessagesService';

const Chat = () => {
  const [messages, setMessages] = useState<Message[]>(mockMessages);
  const messagesService = MessagesService.getInstance();

  useEffect(() => {
    messagesService.fetchMessages().then((fetchedMessages) => {
      setMessages(fetchedMessages);
    });
  }, []);

  const handleSendMessage = async (content: string) => {
    const newMessage: Message = {
      id: `${messages.length + 1}`,
      type: 'text',
      content,
      isMine: true,
    };
    try {
      // Send the message to the API
      const savedMessage = await messagesService.addMessage(newMessage);

      // Update the local state with the saved message from the API
      setMessages([...messages, savedMessage]);
    } catch (error) {
      alert('Failed to send message. Please try again.');
    }
  };

  const handleDeleteMessage = (id: string) => {
    setMessages((prevMessages) => prevMessages.filter((msg) => msg.id !== id));
  };
  

  return (
    <div className="min-h-screen flex flex-col items-center px-4 py-6">
      <div className="w-full max-w-2xl">
        <MessageList messages={messages} onDeleteMessage={handleDeleteMessage} />
        <div className="h-20" />
        <div className="sticky bottom-0 pt-4 z-10 shadow-t">
          <MessageInput onSend={handleSendMessage} />
        </div>
      </div>
    </div>
  );
};

export default Chat;