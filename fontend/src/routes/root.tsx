import { Outlet, useNavigate, useLocation } from 'react-router-dom';
import { Button } from '@/components/ui/button';
import {
  LayoutDashboard,
  Settings,
  Users,
  LogOut,
  Menu,
  Home,
} from 'lucide-react';
import { useEffect, useState } from 'react';
import { getAuthState, logoutAction, hasRole } from '@/lib/auth';
import { cn } from '@/lib/utils';
import { ThemeToggle } from '@/components/theme-toggle';

export default function Root() {
  const navigate = useNavigate();
  const location = useLocation();
  const [sidebarOpen, setSidebarOpen] = useState(true);
  const { user } = getAuthState();

  useEffect(() => {
    if (!user) {
      navigate('/login');
    }
  }, [user, navigate]);

  const handleLogout = async () => {
    await logoutAction();
    navigate('/login');
  };

  const isADMIN = hasRole(['ADMIN']);

  if (!user) {
    return null;
  }

  return (
    <div className="min-h-screen bg-background">
      <div className="flex h-screen">
        {/* Sidebar */}
        <div
          className={cn(
            'border-r bg-background transition-all duration-300 flex flex-col',
            sidebarOpen ? 'w-64' : 'w-16'
          )}
        >
          {/* Header */}
          <div className="flex h-14 items-center border-b px-2">
            <Button
              variant="secondary"
              size="sm"
              className="ml-1 hover:bg-accent/50"
              onClick={() => setSidebarOpen(!sidebarOpen)}
            >
              <Menu className="h-5 w-5" />
            </Button>
            {sidebarOpen && (
              <span className="ml-3 text-lg font-semibold">Dashboard</span>
            )}
          </div>

          {/* Navigation */}
          <div className="flex-1 space-y-1 p-2">
            <Button
              variant="secondary"
              className={cn(
                'w-full justify-start gap-3 text-sm font-normal hover:bg-accent/50',
                location.pathname === '/' && 'bg-accent/50 font-medium'
              )}
              onClick={() => navigate('/')}
            >
              <Home className="h-5 w-5" />
              {sidebarOpen && <span>Home</span>}
            </Button>

            {isADMIN && (
              <>
                <Button
                  variant="secondary"
                  className={cn(
                    'w-full justify-start gap-3 text-sm font-normal hover:bg-accent/50',
                    location.pathname === '/dashboard' && 'bg-accent/50 font-medium'
                  )}
                  onClick={() => navigate('/dashboard')}
                >
                  <LayoutDashboard className="h-5 w-5" />
                  {sidebarOpen && <span>Dashboard</span>}
                </Button>

                <Button
                  variant="secondary"
                  className={cn(
                    'w-full justify-start gap-3 text-sm font-normal hover:bg-accent/50',
                    location.pathname === '/users' && 'bg-accent/50 font-medium'
                  )}
                  onClick={() => navigate('/users')}
                >
                  <Users className="h-5 w-5" />
                  {sidebarOpen && <span>Users</span>}
                </Button>

                <Button
                  variant="secondary"
                  className={cn(
                    'w-full justify-start gap-3 text-sm font-normal hover:bg-accent/50',
                    location.pathname === '/settings' && 'bg-accent/50 font-medium'
                  )}
                  onClick={() => navigate('/settings')}
                >
                  <Settings className="h-5 w-5" />
                  {sidebarOpen && <span>Settings</span>}
                </Button>
              </>
            )}
          </div>

          {/* Footer */}
          <div className="border-t p-2">
            <div className="mb-2">
              <ThemeToggle />
            </div>
            <Button
              variant="secondary"
              className="w-full justify-start gap-3 text-sm font-normal text-muted-foreground hover:bg-accent/50 hover:text-foreground"
              onClick={handleLogout}
            >
              <LogOut className="h-5 w-5" />
              {sidebarOpen && <span>Logout</span>}
            </Button>
          </div>
        </div>

        {/* Main Content */}
        <div className="flex-1 overflow-auto">
          <div className="p-8">
            <Outlet />
          </div>
        </div>
      </div>
    </div>
  );
}