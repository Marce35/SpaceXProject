import { Routes } from '@angular/router';
import {LoginComponent} from "./components/auth/login/login.component";
import {RegisterComponent} from "./components/auth/register/register.component";
import {HomeComponent} from "./components/home/home.component";
import {LaunchesComponent} from "./components/launches/launches/launches.component";
import {AuthGuard} from "./guards/auth-guard";
import {GuestGuard} from "./guards/guest-guard";
import {OutOfServiceComponent} from "./components/out-of-service/out-of-service.component";

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [GuestGuard] // Only for guests
  },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: [GuestGuard] // Only for guests
  },
  {
    path: 'launches',
    component: LaunchesComponent,
    canActivate: [AuthGuard] // Only for users
  },
  { path: 'out-of-service', component: OutOfServiceComponent},
  { path: '**', redirectTo: '' }
];
