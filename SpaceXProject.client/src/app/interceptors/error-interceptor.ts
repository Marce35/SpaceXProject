import {HttpErrorResponse, HttpInterceptorFn} from "@angular/common/http";
import {inject} from "@angular/core";
import {NotificationService} from "../services/notification.service";
import {catchError, throwError} from "rxjs";
import {Router} from "@angular/router";
import {SKIP_ERROR_HANDLING} from "../core/http-context.tokens";

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notificationService = inject(NotificationService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {

      if (req.context.get(SKIP_ERROR_HANDLING)) {
        return throwError(() => error);
      }

      if (error.status === 0 || error.status === 503) {
        if (!router.url.includes('/out-of-service')) {
          router.navigate(['/out-of-service']);
        }
        return throwError(() => error);
      }

      let errorMessage = 'An unexpected error occurred.';

      if (error.status !== 401) {
        notificationService.showError(errorMessage);
      }

      return throwError(() => error);
    })
  );
}
