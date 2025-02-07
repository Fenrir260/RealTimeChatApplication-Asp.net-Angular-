import { Injectable } from '@angular/core';
import { Enviroment } from '../../enviroment/enviroment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CookieService {

  private _apiUrl = `${Enviroment.apiUrl}/cookie`;
  //https://localhost:7071/api/Cookie/getTokenFromCookieByKey/jwtToken

  constructor(
    private _http: HttpClient,
  ) { }

  getDecodedTokenFromCookie(tokenKey: string): Observable<any> {
    return this._http.get<any>(`${this._apiUrl}/getDecodedTokenFromCookieByKey/${tokenKey}`, {
        withCredentials: true,
    });
  } 
  
  getEncodedTokenFromCookie(tokenKey: string) {
    return this._http.get(`${this._apiUrl}/getEncodedTokenFromCookieByKey/${tokenKey}`, {
      withCredentials: true,
    });
  }

  getJwtTokenFromCookie(): string | null {
    const name = 'jwtToken=';
    const decodedCookies = document.cookie;
    const cookieArr = decodedCookies.split(';');
  
    for (let i = 0; i < cookieArr.length; i++) {
      let cookie = cookieArr[i].trim();
      if (cookie.indexOf(name) === 0) {
        return cookie.substring(name.length).trim(); 
      }
    }
    return null;
  }
  
}
