import { redirect } from 'react-router-dom';
import { axiosInstance } from './axios';

export type Role = 'Admin' | 'Customer';

export type User = {
  id: string;
  email: string;
  name: string;
  roles: Role[];
};

export type AuthState = {
  user: User | null;
  token: string | null;
};

export type LoginResponse = {
  token: string;
  isSuccess: boolean;
  message: string;
  roles: Role[];
};

export async function loginAction({ email, password }: { email: string; password: string }) {
  try {
    const response = await axiosInstance.post<LoginResponse>('/api/Account/login', {
      email,
      password,
    });

    const { token, roles } = response.data;

    // Parse JWT to get user info
    const payload = JSON.parse(atob(token.split('.')[1]));
    
    const user: User = {
      id: payload.nameid,
      email: payload.email,
      name: payload.name,
      roles,
    };

    const auth = { user, token };
    localStorage.setItem('auth', JSON.stringify(auth));
    return auth;
  } catch (error) {
    console.error('Login error:', error);
    throw new Error('Invalid credentials');
  }
}

export async function logoutAction() {
  localStorage.removeItem('auth');
}

export function getAuthState(): AuthState {
  try {
    const auth = localStorage.getItem('auth');
    if (auth) {
      return JSON.parse(auth);
    }
  } catch (error) {
    console.error('Error parsing auth state:', error);
  }
  return { user: null, token: null };
}

export function hasRole(requiredRoles: Role[]): boolean {
  const { user } = getAuthState();
  if (!user || !user.roles) return false;
  return user.roles.some(role => requiredRoles.includes(role));
}

export function protectRoute(requiredRoles: Role[]) {
  return async () => {
    const { user } = getAuthState();
    
    if (!user) {
      return redirect('/login');
    }

    if (!hasRole(requiredRoles)) {
      return redirect('/');
    }

    return null;
  };
}