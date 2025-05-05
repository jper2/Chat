import { Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Chat from './components/chat';
import Login from './components/login';
import Register from './components/register';

function App() {
  return (
    <div className="min-h-screen flex flex-col">
      <Navbar />
      <Routes>
        <Route path="/" element={<Chat />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Routes>
    </div>
  );
}

export default App;