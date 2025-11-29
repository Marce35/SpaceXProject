import {HttpInterceptorFn} from "@angular/common/http";

export const credentialsInterceptor: HttpInterceptorFn = (req, next) => {
  const autReq = req.clone({
    withCredentials: true
  });
  return next(autReq);
}
