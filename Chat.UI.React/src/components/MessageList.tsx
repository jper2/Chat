import { Message } from '../types/Message';
import TextMessage from './messages/TextMessage';
import ImageMessage from './messages/ImageMessage';
import ChartMessage from './messages/ChartMessage';
import TableMessage from './messages/TableMessage';
import { useEffect, useRef } from 'react';

interface Props {
  messages: Message[];
}

const MessageList: React.FC<Props> = ({ messages }) => {
  const bottomRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  return (
    <div className="message-list">
      {messages.map((msg) => {
        switch (msg.type) {
          case 'text':
            return <TextMessage key={msg.id} content={msg.content} isMine={msg.isMine} />;
          case 'image':
            return <ImageMessage key={msg.id} url={msg.content} />;
          case 'chart':
            return <ChartMessage key={msg.id} data={msg.content} />;
          case 'table':
            return <TableMessage key={msg.id} markdown={msg.content} />;
          default:
            return null;
        }
      })}
      <div ref={bottomRef} />
    </div>
  );
};

export default MessageList;
