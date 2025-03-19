import { createBrowserRouter } from 'react-router-dom';
import AppLayout from './layouts/AppLayout';
import Home from './Home';
import Contact from './Contact';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <AppLayout />,
    children: [
      {
        index: true,
        element: <Home />,
      },
      {
        path: '/Contact',
        element: <Contact />,
      },
    ],
  },
]);
