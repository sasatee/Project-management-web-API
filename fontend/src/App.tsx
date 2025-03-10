import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import { Toaster } from '@/components/ui/toaster';
import { ThemeProvider } from '@/components/theme-provider';
import Root from './routes/root';
import Login from './routes/login';
import Dashboard from './routes/dashboard';
import Users from './routes/users';
import Settings from './routes/settings';
import Home from './routes/home';
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
        loader: protectRoute(['Customer', 'ADMIN']),
      },
      {
        path: 'dashboard',
        element: <Dashboard />,
        loader: protectRoute(['ADMIN']),
      },
      {
        path: 'users',
        element: <Users />,
        loader: protectRoute(['ADMIN']),
      },
      {
        path: 'settings',
        element: <Settings />,
        loader: protectRoute(['ADMIN']),
      },
    ],
  },
]);

function App() {
  return (
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
      <RouterProvider router={router} />
      <Toaster />
    </ThemeProvider>
  );
}

export default App;