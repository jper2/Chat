// import React from 'react';
// import ReactMarkdown from 'react-markdown';
// import remarkGfm from 'remark-gfm';

// interface Props {
//   content: string;
//   isMine: boolean;
// }

// const TextMessage: React.FC<Props> = ({ content, isMine }) => {
//   return (
//     <div
//       className={`flex ${
//         isMine ? 'justify-end' : 'justify-start'
//       } mb-2`}
//     >
//       <div
//         className={`p-3 rounded shadow-sm max-w-[75%] ${
//           isMine
//             ? 'bg-gray-100 text-gray-800 self-end text-left' // Align text to the left for your messages
//             : ''//bg-gray-100 text-gray-800 text-left' // Align text to the left for others' messages
//         }`}
//       >
//         <div className="prose prose-sm break-words whitespace-pre-wrap">
//           <ReactMarkdown
//             remarkPlugins={[remarkGfm]}
//             components={{
//               p: ({ node, children }) => <p className="mb-2">{children}</p>,
//             }}
//           >
//             {content}
//           </ReactMarkdown>
//         </div>
//       </div>
//     </div>
//   );
// };

// export default TextMessage;


// // import React from 'react';
// // import ReactMarkdown from 'react-markdown';
// // import remarkGfm from 'remark-gfm';

// // interface Props {
// //   content: string;
// // }

// // const TextMessage: React.FC<Props> = ({ content }) => {
// //   return (
// //     <div className="p-3 rounded bg-gray-100 shadow-sm">
// //       <div className="prose max-w-none markdown">
// //         <ReactMarkdown remarkPlugins={[remarkGfm]}>
// //           {content}
// //         </ReactMarkdown>
// //       </div>
// //     </div>
// //   );
// // };

// // export default TextMessage;

import React from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { Trash2 } from 'lucide-react';
import { useAuth } from '../context/AuthContext';

interface Props {
  content: string;
  isOwner: boolean;
  onDelete: () => void; // Callback for delete action
}

const TextMessage: React.FC<Props> = ({ content, isOwner, onDelete }) => {
  return (
    <div
      className={`flex ${isOwner ? 'justify-end' : 'justify-start'} mb-2 group`}
    >
      <div
        className={`relative p-3 rounded shadow-sm max-w-[75%] ${
          isOwner
            ? 'bg-gray-100 text-gray-800 self-end text-left'
            : 'bg-blue-100 text-gray-800 text-left'
        }`}
      >
        {/* Delete Button */}
        <button
          onClick={onDelete}
          className="absolute top-2 right-2 p-1 rounded hover:bg-red-100 dark:hover:bg-red-800 text-gray-400 hover:text-red-500 transition-opacity opacity-0 group-hover:opacity-100"
          title="Delete message"
        >
          <Trash2 size={14} />
        </button>

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