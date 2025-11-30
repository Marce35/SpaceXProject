import {Component, inject} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router, RouterLink} from '@angular/router';
import {MatCardModule} from '@angular/material/card';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {AuthService} from "../../../services/auth.service";
import {LoginRequest} from "../../../data/requests/login-request";
import {ResultStatus} from "../../../data/models/result-pattern/result";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterLink,
    MatCardModule, MatInputModule, MatButtonModule, MatIconModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  fb = inject(FormBuilder);
  authService = inject(AuthService);
  router = inject(Router);

  loginForm: FormGroup;
  hidePassword = true;
  isLoading = false;
  errorMessage = '';

  constructor() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  async onSubmit() {
    if (this.loginForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    const request: LoginRequest = this.loginForm.value as LoginRequest;

    const res = await this.authService.login(request);

    this.isLoading = false;

    if(res.isSuccess){
      this.router.navigate(['/launches']);
    } else{
      if(res.status === ResultStatus.UnAuthorized){
        this.errorMessage = 'Invalid credentials.';
      }else{
        this.errorMessage = res.error?.messages[0] || 'Login failed.';
      }
    }
  }
}
