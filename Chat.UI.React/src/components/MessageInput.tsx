import React, { useState } from 'react';
import { SendHorizonal } from 'lucide-react'; // icon (optional)
import { useAuth } from './context/AuthContext'; 
interface Props {
  onSend: (content: string) => void;
}

const MessageInput: React.FC<Props> = ({ onSend }) => {
  const [input, setInput] = useState('');
  const { isAuthenticated } = useAuth();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const trimmed = input.trim();
    if (!trimmed) return;
    onSend(trimmed);
    setInput('');
  };

  return (
    <form
      onSubmit={handleSubmit}
      className={`flex items-center border rounded-lg px-3 py-2 shadow-sm ${
        isAuthenticated ? 'bg-white focus-within:ring-2 ring-blue-300' : 'bg-gray-200'
      }`}
    >
      <textarea
        className="flex-1 resize-none outline-none text-sm p-1 min-h-[40px] max-h-[120px] overflow-y-auto bg-transparent /*text-white*/ placeholder-gray-400"
        rows={1}
        placeholder={isAuthenticated ? "Type a message...": "Authenticate to chat"}
        value={input}
        onChange={(e) => setInput(e.target.value)}
        onKeyDown={(e) => {
          if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            const trimmed = input.trim();
            if (!trimmed) return;
            onSend(trimmed);
            setInput('');
          }
        }}
      />
      <button type="submit" className="ml-2 text-blue-500 hover:text-blue-600">
        <SendHorizonal size={18} />
      </button>
    </form>
  );
};

export default MessageInput;
