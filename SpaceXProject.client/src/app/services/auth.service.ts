import {inject, Injectable, signal} from '@angular/core';
import {AuthState} from "../data/models/auth-state";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {RegisterRequest} from "../data/requests/register-request";
import {LoginRequest} from "../data/requests/login-request";
import {UserResponse} from "../data/responses/user-response";
import {DbService} from "./db.service";
import {Result} from "../data/models/result-pattern/result";
import {toPromise} from "../core/api-helper";

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7200/api/user';

  private http = inject(HttpClient);
  private router = inject(Router);
  private dbService = inject(DbService);

  authState = signal<AuthState>({isAuthenticated: false, user: null});

  constructor() {
  }

  public async initSession() : Promise<void> {
    const result = await this.checkSession();
    if(result.isSuccess && result.value){
      this.authState.set({isAuthenticated: true, user: result.value});
    }
  }

  async register(data: RegisterRequest): Promise<Result<string>> {
    const request = this.http.post<Result<string>>(`${this.apiUrl}/register`, data);
    return await toPromise(request);
  }

  async login(data: LoginRequest): Promise<Result<UserResponse>> {
    const request = this.http.post<Result<UserResponse>>(`${this.apiUrl}/login`, data);
    const result = await toPromise(request);

    if(result.isSuccess && result.value){
      this.authState.set({
        isAuthenticated: true,
        user: result.value
      });
    }
    return result;
  }

  async logout(): Promise<Result<any>> {
    const request = this.http.post<Result<string>>(`${this.apiUrl}/logout`, {});
    const result = await toPromise(request);

    this.authState.set({isAuthenticated: false, user: null});
    await this.dbService.clearAllSettings();
    await this.router.navigate(['/']);

    return result;
  }

  async checkSession(): Promise<Result<UserResponse>>{
    const request = this.http.get<Result<UserResponse>>(`${this.apiUrl}/check-session`);
    return await toPromise(request);
  }
}
