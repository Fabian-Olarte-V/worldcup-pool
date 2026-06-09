import { authFeatureKey, authReducer } from '../features/auth/store/auth.reducer';

export const appReducers = {
  [authFeatureKey]: authReducer,
};
