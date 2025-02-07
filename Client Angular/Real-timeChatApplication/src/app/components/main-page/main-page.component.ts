import { Component, OnInit } from '@angular/core';
import { FriendshipService } from '../../services/friendship.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UsersService } from '../../services/users.service';
import { UserDTO } from '../../../DTO/userDTO';
import { FindedUsersDTO } from '../../../DTO/findedUsersDTO';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent {

  constructor(
    private _friendshipService: FriendshipService,
    private _userService: UsersService
    ){

  }

  friendsRequestList: UserDTO[] = [];
  sendedFriendRequestList: UserDTO[] = [];
  users: UserDTO[] = [];
  usersFinded: FindedUsersDTO[] = [];
  userNameToFind: string = "";

  async findFriendsByUserName() : Promise<void> {
    if (!this.userNameToFind.trim()) {
      console.warn('Поле для пошуку не може бути порожнім.');
      return;
    }

    await this._friendshipService.findUsersByUserName(this.userNameToFind).subscribe({
      next: (users: UserDTO[]) => 
      {
        this.users = users;
      },
      error: (err) => 
      {
        console.error('Помилка при пошуку користувачів: ', err);
      }
    });
    await this._friendshipService.getSendFriendsRequestList().subscribe({
      next: (response) => 
      {
        this.sendedFriendRequestList = response;
      },
      error: (err) =>
      {
        console.error('Помилка при пошуку користувачів: ', err);
      }
    });
    await this._friendshipService.getFriendsRequestList().subscribe({
      next: (response) =>
      {
        this.friendsRequestList = response;
      },
      error: (err) =>
      {
        console.error('Помилка при пошуку користувачів: ', err);
      }
    });

    await this.findedUsers();
  }

  async findedUsers() : Promise<void>{
    this.usersFinded = this.users.map((user) => {
      const sendedRequest = this.sendedFriendRequestList.some(
        (sended) => sended.userName === user.userName
      );
  
      const gettingRequest = this.friendsRequestList.some(
        (received) => received.userName === user.userName
      );
  
      return {
        userName: user.userName,
        email: user.email,
        authorized: user.authorized,
        gettingRequest: gettingRequest,
        sendedRequest: sendedRequest,
      };
    });
  
    console.log("Знайдені користувачі з оновленими статусами:", this.usersFinded);
  } 

  sendFriendRequest(user: UserDTO) : void {
    this._friendshipService.sendFriendRequest(user).subscribe(
      {
        next: (response) => 
        {
          //Request was send successful.
          console.log(`${response}`);
        },
        error: (err) =>
        {
          console.error(err);
        }
      }
    );
  }

  async cancelFriendRequest(toUser: FindedUsersDTO) : Promise<void> {
    let user: UserDTO = {
      userName: toUser.userName,
      email: toUser.email,
      authorized: toUser.authorized
    };

    await this._friendshipService.cancelFriendRequest(user).subscribe(
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

  async acceptFriendRequest(toUser: FindedUsersDTO) : Promise<void> {
    let user: UserDTO = {
      userName: toUser.userName,
      email: toUser.email,
      authorized: toUser.authorized
    };

    await this._friendshipService.acceptFriendRequest(user).subscribe(
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

  async declineFriendRequest(toUser: FindedUsersDTO) : Promise<void> {
    let user: UserDTO = {
      userName: toUser.userName,
      email: toUser.email,
      authorized: toUser.authorized
    };

    await this._friendshipService.declineFriendRequest(user).subscribe(
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


}
