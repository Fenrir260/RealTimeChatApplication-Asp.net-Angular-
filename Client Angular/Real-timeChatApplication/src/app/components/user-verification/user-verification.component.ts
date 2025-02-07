import { Component, OnInit } from '@angular/core';
import { User } from '../../../models/user';
import { UsersService } from '../../services/users.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserRegisterDTO } from '../../../DTO/userRegisterDTO';
import { CommonModule, ɵparseCookieValue } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserDTO } from '../../../DTO/userDTO';
import { AppComponent } from "../../app.component";

@Component({
  selector: 'app-user-verification',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-verification.component.html',
  styleUrls: ['./user-verification.component.css']
})
export class UserVerificationComponent implements OnInit {

  countdown: number = 10; 
  interval: any;

  constructor(
    private usersService: UsersService, 
    private activatedRoute: ActivatedRoute, 
    private router: Router
  ) {}

  redirectToLogin(): void {
    this.router.navigate(['/login']);
  }

  ngOnInit(): void {
    const userNameFromUrl = this.activatedRoute.snapshot.paramMap.get('userName');
  
    if (userNameFromUrl) {
      this.usersService.setUserAuthorize(userNameFromUrl).subscribe({
        next: () => {
          this.interval = setInterval(() => {
            if (this.countdown > 0) {
              this.countdown--;
            } else {
              this.redirectToLogin();
            }
          }, 1000);
        },
        error: (err) => {
          console.error('Error details:', err.error);
        },
      });
    } else {
      console.error('User name is missing in the URL');
    }
  }
  
  
}
