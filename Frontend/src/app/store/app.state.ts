import { AuthState } from '../features/auth/store/auth.models';

export interface AppState {
  auth: AuthState;
}
