import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  getAllBooks(limit: number, skip: number, search: string) {
    return this.http.get<any>(this.baseUrl + 'books', {
      params: {
        limit,
        skip,
        search,
      },
    });
  }

  getBookByCode(code: string) {
    return this.http.get<any>(this.baseUrl + `books/${code}`);
  }

  storeBook(data: any) {
    return this.http.post<any>(this.baseUrl + 'books', data);
  }

  deleteBookByCode(code: string) {
    return this.http.delete<any>(this.baseUrl + `books/${code}`);
  }
}
