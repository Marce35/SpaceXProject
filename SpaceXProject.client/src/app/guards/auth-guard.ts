import {CanActivateFn, Router} from "@angular/router";
import {inject} from "@angular/core";
import {AuthService} from "../services/auth.service";

export const AuthGuard: CanActivateFn = async (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  await authService.sessionCheckPromise;

  if(authService.authState().isAuthenticated){
    return true;
  }
  router.navigate(['/login']);
  return false;
}
