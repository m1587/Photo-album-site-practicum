
import { RouterProvider } from 'react-router-dom';
import './App.css'
import { UserProvider } from './context/UserContext';
import { router } from './Router';

function App() {
  return (
    <UserProvider>
      <RouterProvider router={router} />
    </UserProvider>
  );
}

export default App
