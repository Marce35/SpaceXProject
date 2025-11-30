import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const GuestGuard: CanActivateFn = async (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Wait for session check to resolve
  await authService.sessionCheckPromise;

  // If user IS authenticated, kick them to Launches (or Home)
  if (authService.authState().isAuthenticated) {
    router.navigate(['/launches']);
    return false;
  }

  // If NOT authenticated, let them proceed to Login/Register
  return true;
};
