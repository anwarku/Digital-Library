import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { jwtDecode, JwtPayload } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  baseUrl: string = environment.baseUrl;
  constructor(private http: HttpClient) {}

  getAllReport() {
    return this.http.get(this.baseUrl + 'reports');
  }

  downloadReportById(id: number): Observable<Blob> {
    return this.http.get(this.baseUrl + `reports/download/${id}`, {
      responseType: 'blob',
    });
  }

  uploadReport(reportDate: string, reportFile: File) {
    // Mendapatkan user id dari token biar lebih aman
    const token = localStorage.getItem('token') as string;
    const decodedToken = jwtDecode<any>(token);
    const userId = decodedToken.userId;

    const formData = new FormData();
    formData.append('userId', userId),
      formData.append('reportDate', reportDate);
    formData.append('file', reportFile);

    return this.http.post<any>(this.baseUrl + 'reports/upload', formData);
  }
}
