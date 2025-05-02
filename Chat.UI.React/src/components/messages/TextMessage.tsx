import React from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';

interface Props {
  content: string;
  isMine: boolean;
}

const TextMessage: React.FC<Props> = ({ content, isMine }) => {
  return (
    <div
      className={`flex ${
        isMine ? 'justify-end' : 'justify-start'
      } mb-2`}
    >
      <div
        className={`p-3 rounded shadow-sm max-w-[75%] ${
          isMine
            ? 'bg-gray-100 text-gray-800 self-end text-left' // Align text to the left for your messages
            : ''//bg-gray-100 text-gray-800 text-left' // Align text to the left for others' messages
        }`}
      >
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


// import React from 'react';
// import ReactMarkdown from 'react-markdown';
// import remarkGfm from 'remark-gfm';

// interface Props {
//   content: string;
// }

// const TextMessage: React.FC<Props> = ({ content }) => {
//   return (
//     <div className="p-3 rounded bg-gray-100 shadow-sm">
//       <div className="prose max-w-none markdown">
//         <ReactMarkdown remarkPlugins={[remarkGfm]}>
//           {content}
//         </ReactMarkdown>
//       </div>
//     </div>
//   );
// };

// export default TextMessage;
