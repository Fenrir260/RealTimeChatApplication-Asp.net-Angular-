import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Enviroment } from '../../enviroment/enviroment';
import { HttpClient } from '@angular/common/http';
import { UserDTO } from '../../DTO/userDTO';
import { tick } from '@angular/core/testing';

@Injectable({
  providedIn: 'root'
})
export class FriendshipService {

  private apiUrl = `${Enviroment.apiUrl}/friends`

  constructor(private http: HttpClient) 
  { 

  }

  findUsersByUserName(userName: string) : Observable<UserDTO[]> {
    return this.http.get<UserDTO[]>(`${this.apiUrl}/findUserByUsersName/${userName}`);
  }

  sendFriendRequest(toUser: UserDTO) : Observable<any>
  {
    return this.http.post<any>(`${this.apiUrl}/sendFriendRequest`, toUser, {
      withCredentials: true,
    });
  }

  acceptFriendRequest(toUser: UserDTO) : Observable<any> 
  {
    return this.http.put<any>(`${this.apiUrl}/acceptFriendRequest`, toUser, {
      withCredentials: true,
    });
  }

  cancelFriendRequest(toUser: UserDTO) : Observable<any> 
  {
    return this.http.post<UserDTO>(`${this.apiUrl}/cancelFriendRequest`, toUser, {
      withCredentials: true,
    })
  }

  declineFriendRequest(toUser: UserDTO) : Observable<any> 
  {
    return this.http.delete<any>(`${this.apiUrl}/declineFriendRequest`, {
      body: toUser,
      withCredentials: true,
    });
  }

  getMyFriendsList() : Observable<any> 
  {
    return this.http.get<any>(`${this.apiUrl}/getMyFriendsList`, {
      withCredentials: true,
    });
  }

  getFriendsRequestList() : Observable<any> 
  {
    return this.http.get<any>(`${this.apiUrl}/getFriendsRequestList`, {
      withCredentials: true,
    });
  }

  getSendFriendsRequestList() : Observable<any> 
  {
    return this.http.get<any>(`${this.apiUrl}/getSendFriendsRequestList`, {
      withCredentials: true,
    });
  }


  
}
