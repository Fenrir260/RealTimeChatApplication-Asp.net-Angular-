import { Injectable } from '@angular/core';
import { Enviroment } from '../../enviroment/enviroment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { UserDTO } from '../../DTO/userDTO';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  private _apiUrl = `${Enviroment.apiUrl}/userprofile`;

  constructor(
    private _http: HttpClient
  ) { }

  saveUserProfile(profileData: any): Observable<any> {
    return this._http.put<any>(`${this._apiUrl}/saveUserProfile`, profileData, {
      headers: { 'Content-Type': 'application/json' },
    });
  }

  uploadUserPhoto(userPhoto: FormData): Observable<any> {
    return this._http.put(`${this._apiUrl}/uploadPhoto`, userPhoto);
  }
  
  getUserProfilePhoto(userName: string): Observable<Blob> {
    return this._http.get(`${this._apiUrl}/getPhotoByUserName/${userName}`, {
      responseType: 'blob',
      withCredentials: true
    });
  }

  getUserProfileData(user: string): Observable<any> {
    return this._http.get(`${this._apiUrl}/getUserProfileData/${user}`, {
      withCredentials: true
    });
  }
  
}
