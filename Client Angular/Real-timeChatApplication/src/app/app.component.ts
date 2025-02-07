import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet, RouterModule, ActivatedRoute, Router} from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { inject, NgModule } from '@angular/core';
import { UserDTO } from '../DTO/userDTO';
import { UsersService } from './services/users.service';
import jwt_decode from 'jwt-decode';
import { CookieService } from './services/cookie.service';
import { userTokenDTO } from '../DTO/userTokenDTO';
import { FriendshipService } from './services/friendship.service';
import { __core_private_testing_placeholder__, tick } from '@angular/core/testing';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule, RouterModule, FormsModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit
{
  coundown: number = 5;
  user: UserDTO = {
    userName: "",
    email: "",
    authorized: false,
  }

  friendsRequestCount : number = 0;
  sendedfriendsRequestCount: number = 0;

  isRequestModalOpen: boolean = false;
  isFriendsModalOpen: boolean = false;

  friendsList: UserDTO[] | undefined;
  friendsRequest: UserDTO[] | undefined;
  sendedfriendsRequest: UserDTO[] | undefined;
  

  constructor(
    private _userService: UsersService, 
    private _activeRoute: ActivatedRoute, 
    private _friendshipService: FriendshipService,
    private _router: Router,
    private _cookieService: CookieService,
    ){

  }

  async ngOnInit(): Promise<void> {
    await this._friendshipService.getFriendsRequestList().subscribe(
      {
        next: (response) => 
        {
          this.friendsRequest = response;
          this.friendsRequestCount = this.friendsRequest ? this.friendsRequest.length : 0;
        },
        error: (err) =>
        {
          console.error(`getFriendsRequestList ${err}`);
        }
      }
    );
    await this._friendshipService.getSendFriendsRequestList().subscribe(
      {
        next: (response) => 
        {
          this.sendedfriendsRequest = response;
          this.sendedfriendsRequestCount = this.sendedfriendsRequest ? this.sendedfriendsRequest.length : 0;
        },
        error: (err) =>
        {
          console.error(`getSendFriendsRequestList ${err}`);
        }
      }
    );
    await this._friendshipService.getMyFriendsList().subscribe(
      {
        next: (response) => 
        {
          this.friendsList = response;
        },
        error: (err) =>
        {
          console.error(err);
        }
      }
    );
    await console.log()
  }

  async cancelFriendRequest(toUser: UserDTO) : Promise<void> {
    await this._friendshipService.cancelFriendRequest(toUser).subscribe(
      {
        next: (response) => 
        {
          console.log(`${response}`);
        },
        error: (err) =>
        {
          console.error(err);
        }
      }
    );
  }

  async acceptFriendRequest(toUser: UserDTO) : Promise<void> {
    await this._friendshipService.acceptFriendRequest(toUser).subscribe(
      {
        next: (response) => 
        {
          console.log(`${response}`);
        },
        error: (err) =>
        {
          console.error(err);
        }
      }
    );
  }

  async declineFriendRequest(toUser: UserDTO) : Promise<void> {
    await this._friendshipService.declineFriendRequest(toUser).subscribe(
      {
        next: (response) =>
        {
          
        },
        error: (err) => 
        {
          console.error(err);
        } 
      }
    );
  }

  closeRequestModal() : void {
    this.isRequestModalOpen = false;
  }

  openRequestModal() : void {
    this.isRequestModalOpen = true;
    this.isFriendsModalOpen = false;
  }

  closeFriendsModal() : void {
    this.isFriendsModalOpen = false;
  }

  openFriendsModal() : void {
    this.isFriendsModalOpen = true;
    this.isRequestModalOpen = false;
  }


  redirectToProfile(): void {
    this._cookieService.getDecodedTokenFromCookie("jwtToken").subscribe({
      next: (response: userTokenDTO) => {
        if (response && response.userName) {
          if (response.userName === null || response.userName === '') {
            console.error("UserName is null or empty");
            return;
          }
          this._router.navigate([`profile/${response.userName}`]);
        } else {
          console.warn('Token data is null or malformed');
        }
      },
      error: (err) => {
        console.error("Error: ", err);
        alert("Помилка при отриманні даних токену.");
      }
    });
  }

  redirectToHomePage() : void {
    this._router.navigate(["home"]);
  }
  

  redirectToLogin() : void {
    this.clearCookies();

    this._router.navigate(['login']);
  }

  clearCookies() : void {
    const cookies = document.cookie.split(';');
    cookies.forEach(cookie => {
      const cookieName = cookie.split('=')[0].trim();
      document.cookie = `${cookieName}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
    });
  }

}


@NgModule({
  providers: [
  
  ]
})

export class AppComponentModule {}