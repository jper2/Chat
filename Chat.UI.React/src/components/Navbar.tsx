import { useAuth } from './context/AuthContext';
import { useNavigate } from 'react-router-dom';

const Navbar = () => {
  const { isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <header className="bg-gray-800 text-white p-4 flex justify-between items-center">
      <h1 className="text-xl font-bold">Chat App</h1>
      <div>
        {isAuthenticated ? (
          <>
            <span className="mr-4">Logged in</span>
            <button
              onClick={handleLogout}
              className="bg-red-500 px-4 py-2 rounded hover:bg-red-600"
            >
              Logout
            </button>
          </>
        ) : (
          <>
            <button
              onClick={() => navigate('/login')}
              className="bg-blue-500 px-4 py-2 rounded hover:bg-blue-600 mr-2"
            >
              Login
            </button>
            <button
              onClick={() => navigate('/register')}
              className="bg-green-500 px-4 py-2 rounded hover:bg-green-600"
            >
              Register
            </button>
          </>
        )}
      </div>
    </header>
  );
};

export default Navbar;