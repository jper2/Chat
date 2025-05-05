import React from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { Trash2 } from 'lucide-react';
import { useAuth } from '../context/AuthContext';

interface Props {
  id?: string; 
  content: string;
  createdByCurrentUser: boolean;
  onDelete: () => void; // Callback for delete action
}

const TextMessage: React.FC<Props> = ({ id, content, createdByCurrentUser: createdByCurrentUser, onDelete }) => {
   const { isAuthenticated } = useAuth();
  return (
    <div
      className={`flex ${createdByCurrentUser ? 'justify-end' : 'justify-start'} mb-2 group`}
    >
      <div
        className={`relative p-3 rounded shadow-sm max-w-[75%] ${
          createdByCurrentUser
            ? 'bg-gray-100 text-gray-800 self-end text-left'
            : ''//bg-blue-100 text-gray-800 text-left
        }`}
      >
        {/* Conditionally Render Delete Button */}
        {id && id.trim() !== '' && !id.startsWith('mock') && isAuthenticated && (
          <button
            onClick={onDelete}
            className="absolute top-2 right-2 p-1 rounded hover:bg-red-100 dark:hover:bg-red-800 text-gray-400 hover:text-red-500 transition-opacity opacity-0 group-hover:opacity-100"
            title="Delete message"
          >
            <Trash2 size={14} />
          </button>
        )}

        {/* Message Content */}
        <div className="prose prose-sm break-words whitespace-pre-wrap">
          <ReactMarkdown
            remarkPlugins={[remarkGfm]}
            components={{
              p: ({ node, children }) => <p className="mb-2">{children}</p>,
            }}
          >
            {content}
          </ReactMarkdown>
        </div>
      </div>
    </div>
  );
};

export default TextMessage;