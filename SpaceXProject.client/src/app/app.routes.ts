import { Routes } from '@angular/router';
import {LoginComponent} from "./components/auth/login/login.component";
import {RegisterComponent} from "./components/auth/register/register.component";
import {HomeComponent} from "./components/home/home.component";
import {LaunchesComponent} from "./components/launches/launches/launches.component";
import {AuthGuard} from "./guards/auth-guard";

export const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'launches', component: LaunchesComponent, canActivate: [AuthGuard]},
  {path: '**', redirectTo: ''}
];
