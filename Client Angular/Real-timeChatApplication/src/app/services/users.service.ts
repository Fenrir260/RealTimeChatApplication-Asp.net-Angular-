import { Injectable } from '@angular/core';
import { Enviroment } from '../../enviroment/enviroment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { UserDTO } from '../../DTO/userDTO';
import { UserRegisterDTO } from '../../DTO/userRegisterDTO';
import { User } from '../../models/user';
import { UserLoginDTO } from '../../DTO/userLoginDTO';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private apiUrl = `${Enviroment.apiUrl}/user`;
  private headers = new HttpHeaders();

  constructor(private http: HttpClient) { }

  private getAuthHeaders(): HttpHeaders {
    const tokenFromCookies = document.cookie.split('; ').find(row => row.startsWith('jwtToken='))?.split('=')[1];
  
    const token = tokenFromCookies || localStorage.getItem('jwtToken');
  
    return new HttpHeaders({
      Authorization: token ? `Bearer ${token}` : ''
    });
  }
  
  getIdByUserName(userName: string) : Observable<number>{
    return this.http.get<number>(`${this.apiUrl}/getIdByUserName/${userName}`);
  }

  loginUser(user: UserLoginDTO): Observable<string>{
    return this.http.post<string>(`${this.apiUrl}/loginUser`, user, {
      responseType: 'text' as 'json',
      withCredentials: true,
      headers: { 'Content-Type': 'application/json',
      }});
  }

  registerUser(user: UserRegisterDTO): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/registerUser`, user, {
      responseType: 'text' as 'json',
      withCredentials: true  
    });;
  } 

  setUserAuthorize(userName: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/setUserAuthorize`, JSON.stringify(userName), {
        headers: { 'Content-Type': 'application/json' },
        responseType: 'text'
    });
}
  
  
  setRefreshToken(user: UserDTO) : Observable<any> {
    return this.http.get(`${this.apiUrl}/${user}/setRefreshToken`);
  }

  getUserByUserName(userName: string): Observable<UserDTO> {
    return this.http.get<UserDTO>(`${this.apiUrl}/getUserByUserName/${userName}`, {
      headers: this.getAuthHeaders(), 
      withCredentials: true
    });
  }

  getUserByEmail(email: string) : Observable<UserDTO> {
    return this.http.get<UserDTO>(`${this.apiUrl}/getUserByEmail/${email}`);
  }
  

  getUsers() : Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/getUsers`);
  }
}
