import {ApplicationConfig, inject, provideAppInitializer} from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import {provideHttpClient, withInterceptors} from "@angular/common/http";
import {credentialsInterceptor} from "./interceptors/credentials-interceptor";
import {AuthService} from "./services/auth.service";

export const appConfig: ApplicationConfig = {
  providers:
    [provideRouter(routes),
    provideHttpClient(withInterceptors([credentialsInterceptor])),

    provideAppInitializer(() => {
      const authService = inject(AuthService);
      return authService.initSession();
    })
  ]
};
