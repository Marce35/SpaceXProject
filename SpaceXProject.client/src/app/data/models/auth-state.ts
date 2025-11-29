import {UserResponse} from "../responses/user-response";

export interface AuthState {
  isAuthenticated: boolean;
  user: UserResponse | null;
}
