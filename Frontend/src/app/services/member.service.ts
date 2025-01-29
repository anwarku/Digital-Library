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

  checkMemberById(memberId: number) {
    return this.http.get<any>(this.baseUrl + `members/check/${memberId}`);
  }

  storeMember(data: any, imageFile: File) {
    const formData = new FormData();

    formData.append('name', data.name);
    formData.append('gender', data.gender);
    formData.append('phone', data.phone);
    formData.append('job', data.job);
    formData.append('imageFile', imageFile);

    return this.http.post<any>(this.baseUrl + 'members', formData);
  }
}
