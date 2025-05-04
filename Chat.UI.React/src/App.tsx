import { useEffect, useState } from 'react';
import MessageList from './components/MessageList';
import MessageInput from './components/MessageInput';
import { mockMessages } from './data/mockMessages';
import { Message } from './types/Message';
import './App.css';
import { MessagesService } from './services/MessagesService';

function App() {
  const [messages, setMessages] = useState<Message[]>(mockMessages);
  const messagesService = MessagesService.getInstance('https://localhost:7117/api');
  useEffect(() => {

    messagesService.fetchMessages()
      .then((fetchedMessages) => {
        setMessages(fetchedMessages);
      });
  }, []);

  const handleSendMessage = (content: string) => {
    const newMessage: Message = {
      id: `${messages.length + 1}`,
      type: 'text',
      content,
      isMine: true,
    };
    setMessages([...messages, newMessage]);
  };

  return (
    <div className="min-h-screen flex flex-col items-center px-4 py-6">
      <div className="w-full max-w-2xl">
        <MessageList messages={messages} />
        <div className="h-20" />
        {/* <div className="mt-4"> */}
        <div className="sticky bottom-0 pt-4 z-10 shadow-t">
          <MessageInput onSend={handleSendMessage} />
        </div>

        {/* </div> */}
      </div>
    </div>
  );
}

export default App;

// import { useEffect, useRef, useState } from 'react';
// import { mockMessages } from './data/mockMessages';
// import MessageList from './components/MessageList';
// import MessageInput from './components/MessageInput';
// import { Message } from './types/Message';

// function App() {
//   const [messages, setMessages] = useState<Message[]>(mockMessages);
//   const messagesEndRef = useRef<HTMLDivElement | null>(null);

//   const handleSendMessage = (content: string) => {
//     const newMessage: Message = {
//       id: `${Date.now()}`,
//       type: 'text',
//       content,
//     };
//     setMessages((prev) => [...prev, newMessage]);
//   };

//   useEffect(() => {
//     messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
//   }, [messages]);

//   return (
//     <div className="flex flex-col h-screen">
//       {/* Scrollable message list */}
//       <main className="flex-1 overflow-y-auto px-4 py-2 bg-gray-50">
//         <div className="max-w-3xl mx-auto">
//           <MessageList messages={messages} />
//           <div ref={messagesEndRef} />
//         </div>
//       </main>

//       {/* Fixed input bar */}
//       <footer className="sticky bottom-0 bg-white border-t px-4 py-2">
//         <div className="max-w-3xl mx-auto">
//           <MessageInput onSend={handleSendMessage} />
//         </div>
//       </footer>
//     </div>
//   );
// }

// export default App;



// import { useState } from 'react'
// import { mockMessages } from './data/mockMessages';
// import MessageList from './components/MessageList';
// import MessageInput from './components/MessageInput';
// import { Message } from './types/Message';
// import './App.css'

// function App() {
//   const [messages, setMessages] = useState<Message[]>(mockMessages);

//   const handleSendMessage = (content: string) => {
//     const newMessage: Message = {
//       id: `${messages.length + 1}`,
//       type: 'text', // Default type for now (can later handle more types like image, etc.)
//       content,
//     };
//     setMessages([...messages, newMessage]); // Add new message to the list
//   };

//   return (
//     <div className="min-h-screen bg-gray-100 p-4 flex flex-col items-center">
//       <div className="w-full max-w-2xl bg-white rounded shadow p-4">
//         <h1 className="text-xl font-semibold mb-4">📬 Multi-Type Chat</h1>
//         <MessageList messages={messages} />
//         <MessageInput onSend={handleSendMessage} />
//       </div>
//     </div>
//   );
//   // return (
//   //   <div className="min-h-screen bg-gray-100 p-4 flex flex-col items-center">
//   //     <div className="w-full max-w-2xl bg-white rounded shadow flex flex-col h-[90vh]">
//   //       <h1 className="text-xl font-semibold p-4 border-b">📬 Multi-Type Chat</h1>
  
//   //       {/* Scrollable message area */}
//   //       <div className="flex-1 overflow-y-auto p-4">
//   //         <MessageList messages={messages} />
//   //       </div>
  
//   //       {/* Fixed input at bottom */}
//   //       <div className="border-t p-4">
//   //         <MessageInput onSend={handleSendMessage} />
//   //       </div>
//   //     </div>
//   //   </div>
//   // );
// }

// export default App
