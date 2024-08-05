import { Routes } from '@angular/router';
import { LoginComponent } from './users/login/login.component';
import { HomeComponent } from './users/home/home.component';
import { authGuard } from './shared/auth/guards/auth.guard';

export const routes: Routes = [
    {
        path: 'login',
        title: 'Login',
        component: LoginComponent,
    },
    {
        path: '',
        title: 'Home page',
        component: HomeComponent, 
        canActivate: [authGuard]
    },
    // TODO: add other routes
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];
