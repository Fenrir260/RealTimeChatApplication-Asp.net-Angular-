import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserRegisterDTO } from '../../../DTO/userRegisterDTO';
import { UsersService } from '../../services/users.service';
import { UserLoginDTO } from '../../../DTO/userLoginDTO';
import { UserDTO } from '../../../DTO/userDTO';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent {
  isRegisterMode: boolean = true; // Перемикач між режимами реєстрації та логіну

  user: UserRegisterDTO = {
    userName: '',
    email: '',
    password: '',
    confirmationUrl: ''
  };

  constructor(private _userService: UsersService, private router: Router) {}

  toggleMode(): void {
    this.isRegisterMode = !this.isRegisterMode;
  }

  handleSubmit(): void {
    if (!this.user.email || !this.user.password) {
      alert('Всі поля є обов’язковими!');
      return;
    }

    if (this.isRegisterMode && !this.user.userName) {
      alert('Всі поля є обов’язковими для реєстрації!');
      return;
    }

    if (this.isRegisterMode) {
      this.registerUser();
    } else {
      this.loginUser();
    }
  }

  private registerUser(): void {
    const confirmationUrl = this.generateConfirmationUrl(this.user.userName);

    localStorage.clear();
    
    const user: UserRegisterDTO = {
      userName: this.user.userName,
      email: this.user.email,
      password: this.user.password,
      confirmationUrl: confirmationUrl
    };

    this._userService.registerUser(user).subscribe({
      next: (jwtToken: string) => {
        alert('Account created successfully! Please check your email.');
      },
      error: (error) => {
        console.error('Error details:', error);
        return;
      }
    });
  }

  private loginUser(): void {

    localStorage.clear();

    const user: UserLoginDTO = {
      email: this.user.email,
      password: this.user.password,
    }
    this._userService.loginUser(user).subscribe({
      next: (response) => {
        this.router.navigate(['/home']);
      },
      error: (error) => {
        console.error('Error details:', error);
        alert('Invalid email or password.');
        return;
      }
    });
  }

  clearCookies() : void {
    const cookies = document.cookie.split(';');
    cookies.forEach(cookie => {
      const cookieName = cookie.split('=')[0].trim();
      document.cookie = `${cookieName}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
    });
  }
  

  private generateConfirmationUrl(userName: string): string {
    const baseUrl = window.location.origin;
    return `${baseUrl}/userVerification/${userName}`;
  }
}
