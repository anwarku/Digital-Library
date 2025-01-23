import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpBackend, HttpClient, HttpContext } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.baseUrl;
  private http: HttpClient;
  constructor(private handler: HttpBackend) {
    // Menimpa header authorization
    // Tidak ada authorization dalam header
    this.http = new HttpClient(handler);
  }

  // Endpoint untuk melakukan login
  userLogin(credentials: any) {
    return this.http.post<any>(this.baseUrl + 'users/login', credentials);
  }

  isLoggedIn() {
    const token = localStorage.getItem('token');
    const nameUser = localStorage.getItem('name');

    if (!token && !nameUser) {
      return false;
    }

    return true;
  }
}
