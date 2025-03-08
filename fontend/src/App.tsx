import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import Root from './routes/root';
import Login from './routes/login';
import Dashboard from './routes/dashboard';
import Users from './routes/users';
import Settings from './routes/settings';
import Home from './routes/home';
import { Toaster } from '@/components/ui/toaster';
import { protectRoute } from './lib/auth';

const router = createBrowserRouter([
  {
    path: '/login',
    element: <Login />,
  },
  {
    path: '/',
    element: <Root />,
    children: [
      {
        index: true,
        element: <Home />,
        loader: protectRoute(['Customer', 'Admin']),
      },
      {
        path: 'dashboard',
        element: <Dashboard />,
        loader: protectRoute(['Admin']),
      },
      {
        path: 'users',
        element: <Users />,
        loader: protectRoute(['Admin']),
      },
      {
        path: 'settings',
        element: <Settings />,
        loader: protectRoute(['Admin']),
      },
    ],
  },
]);

export default function App() {
  return (
    <>
      <RouterProvider router={router} />
      <Toaster />
    </>
  );
}