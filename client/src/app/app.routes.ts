import { Routes } from '@angular/router';
import { LoginComponent } from './users/login/login.component';
import { HomeComponent } from './users/home/home.component';
import { authGuard } from './shared/auth/guards/auth.guard';
import { provideState } from '@ngrx/store';
import { userFeatureKey, userReducer } from './users/store/user.reducer';

export const routes: Routes = [
    {
        path: 'login',
        title: 'Login',
        component: LoginComponent,
        providers: [
            provideState({ name: userFeatureKey, reducer: userReducer })
          ]
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
