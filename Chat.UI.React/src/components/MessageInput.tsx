import React, { useState } from 'react';
import { SendHorizonal } from 'lucide-react'; // icon (optional)

interface Props {
  onSend: (content: string) => void;
}

const MessageInput: React.FC<Props> = ({ onSend }) => {
  const [input, setInput] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const trimmed = input.trim();
    if (!trimmed) return;
    onSend(trimmed);
    setInput('');
  };

  return (
    <form onSubmit={handleSubmit} className="flex items-center border rounded-lg px-3 py-2 shadow-sm bg-white focus-within:ring-2 ring-blue-300">
      <textarea
        className="flex-1 resize-none outline-none text-sm p-1 min-h-[40px] max-h-[120px] overflow-y-auto bg-transparent /*text-white*/ placeholder-gray-400"
        rows={1}
        placeholder="Type a message..."
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

// import React, { useState } from 'react';

// interface Props {
//   onSend: (content: string) => void;
// }

// const MessageInput: React.FC<Props> = ({ onSend }) => {
//   const [message, setMessage] = useState('');

//   const handleChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
//     setMessage(e.target.value);
//   };

//   const handleSend = () => {
//     if (message.trim()) {
//       onSend(message);
//       setMessage(''); // Clear input after sending
//     }
//   };

//   return (
//     <div className="message-input mt-4">
//       <textarea
//         value={message}
//         onChange={handleChange}
//         rows={4}
//         className="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
//         placeholder="Type your message..."
//       />
//       <button
//         onClick={handleSend}
//         className="mt-2 w-full bg-blue-500 text-white py-2 rounded hover:bg-blue-600"
//       >
//         Send Message
//       </button>
//     </div>
//   );
// };

// export default MessageInput;
