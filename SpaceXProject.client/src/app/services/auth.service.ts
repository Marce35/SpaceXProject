import {Injectable, signal} from '@angular/core';
import {AuthState} from "../data/models/auth-state";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {RegisterRequest} from "../data/requests/register-request";
import {LoginRequest} from "../data/requests/login-request";
import {catchError, Observable, of, tap} from "rxjs";
import {UserResponse} from "../data/responses/user-response";

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7200/api/user';

  authState = signal<AuthState>({isAuthenticated: false, user: null});

  constructor(private http: HttpClient, private router: Router) {
    this.checkSession().subscribe();
  }

  register(data: RegisterRequest){
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  login(data: LoginRequest){
    return this.http.post(`${this.apiUrl}/login`, data).pipe(
      tap(() => this.checkSession().subscribe())
    )
  }

  logout(){
    return this.http.post(`${this.apiUrl}/logout`, {}).pipe(
    tap(() => {
      this.authState.set({isAuthenticated: false, user: null});
      this.router.navigate(['/login']);
    })
    );
  }

  checkSession(): Observable<UserResponse | null>{
    return this.http.get<UserResponse>(`${this.apiUrl}/check-session`).pipe(
      tap((userResponse) => {
        this.authState.set({isAuthenticated: true, user: userResponse});
      }),
      catchError(() => {
        this.authState.set({isAuthenticated: false, user: null});
        return of(null);
      })
    )
  }
}
