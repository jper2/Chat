import React from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';

interface Props {
  markdown: string;
}

const TableMessage: React.FC<Props> = ({ markdown }) => {
  return (
    <div className="p-3 rounded bg-white shadow-sm overflow-x-auto">
      <div className="prose max-w-none markdown">
        <ReactMarkdown
          remarkPlugins={[remarkGfm]}
          components={{
            table: ({ children }) => (
              <table className="min-w-full border border-gray-300">
                {children}
              </table>
            ),
            thead: ({ children }) => <thead className="bg-gray-100">{children}</thead>,
            th: ({ children }) => (
              <th className="border border-gray-300 px-2 py-1 text-left font-medium">
                {children}
              </th>
            ),
            td: ({ children }) => (
              <td className="border border-gray-200 px-2 py-1">{children}</td>
            ),
          }}
        >
          {markdown}
        </ReactMarkdown>
      </div>
    </div>
  );
};

export default TableMessage;
