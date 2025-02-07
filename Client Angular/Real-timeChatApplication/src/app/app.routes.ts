import { Routes } from '@angular/router';
import { AuthComponent } from './components/auth/auth.component';
import { UserVerificationComponent } from './components/user-verification/user-verification.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';

export const routes: Routes = [
    { path: 'register', component: AuthComponent },
    { path: 'login', component: AuthComponent },
    { path: '', component: MainPageComponent },
    { path: 'home', component: MainPageComponent},
    { path: 'userVerification/:userName', component: UserVerificationComponent },
    { path: 'profile/:userName', component: UserProfileComponent }
];
