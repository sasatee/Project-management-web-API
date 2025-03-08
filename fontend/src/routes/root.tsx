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

  const isAdmin = hasRole(['Admin']);

  if (!user) {
    return null;
  }

  return (
    <div className="min-h-screen bg-background">
      <div className="flex h-screen">
        {/* Sidebar */}
        <div
          className={cn(
            'bg-card border-r p-4 transition-all duration-300',
            sidebarOpen ? 'w-64' : 'w-20'
          )}
        >
          <div className="flex items-center gap-2 mb-8">
            <Button
              variant="ghost"
              size="icon"
              onClick={() => setSidebarOpen(!sidebarOpen)}
            >
              <Menu className="h-6 w-6" />
            </Button>
            {sidebarOpen && <h1 className="text-xl font-bold">Dashboard</h1>}
          </div>
          
          <nav className="space-y-2">
            <Button
              variant="ghost"
              className={cn(
                'w-full justify-start',
                location.pathname === '/' && 'bg-accent'
              )}
              onClick={() => navigate('/')}
            >
              <Home className="h-4 w-4" />
              {sidebarOpen && <span className="ml-2">Home</span>}
            </Button>

            {isAdmin && (
              <>
                <Button
                  variant="ghost"
                  className={cn(
                    'w-full justify-start',
                    location.pathname === '/dashboard' && 'bg-accent'
                  )}
                  onClick={() => navigate('/dashboard')}
                >
                  <LayoutDashboard className="h-4 w-4" />
                  {sidebarOpen && <span className="ml-2">Dashboard</span>}
                </Button>
                
                <Button
                  variant="ghost"
                  className={cn(
                    'w-full justify-start',
                    location.pathname === '/users' && 'bg-accent'
                  )}
                  onClick={() => navigate('/users')}
                >
                  <Users className="h-4 w-4" />
                  {sidebarOpen && <span className="ml-2">Users</span>}
                </Button>
                
                <Button
                  variant="ghost"
                  className={cn(
                    'w-full justify-start',
                    location.pathname === '/settings' && 'bg-accent'
                  )}
                  onClick={() => navigate('/settings')}
                >
                  <Settings className="h-4 w-4" />
                  {sidebarOpen && <span className="ml-2">Settings</span>}
                </Button>
              </>
            )}
          </nav>

          <div className="absolute bottom-4" style={{ width: sidebarOpen ? '14rem' : '4rem' }}>
            <Button
              variant="ghost"
              className="w-full justify-start text-destructive"
              onClick={handleLogout}
            >
              <LogOut className="h-4 w-4" />
              {sidebarOpen && <span className="ml-2">Logout</span>}
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