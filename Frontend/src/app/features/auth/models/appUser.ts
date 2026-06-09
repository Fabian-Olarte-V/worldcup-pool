export type UserRole = 'ADMIN' | 'USER';

export interface AuthUser {
  userId: string;
  token: string;
  refreshToken: string;
  expiresAtUtc: string;
}

export interface AppUser {
  id: string;
  username?: string | null;
  role?: UserRole | null;
}

export interface AuthRequestPayload {
  username: string;
  password: string;
}

export interface SignupRequestPayload {
  firstName: string;
  lastName: string;
  email: string;
  username: string;
  password: string;
  role: UserRole;
}
