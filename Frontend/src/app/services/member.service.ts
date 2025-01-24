import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  // Endpoint untuk mendapatkan data member berdasarkan ID Member
  getMemberById(memberId: number) {
    return this.http.get<any>(this.baseUrl + `members/${memberId}`);
  }
}
