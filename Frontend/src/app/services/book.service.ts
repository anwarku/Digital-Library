import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  // Endpoint untuk mendapatkan semua data buku
  getAllBooks(limit: number, skip: number, search: string) {
    return this.http.get<any>(this.baseUrl + 'books', {
      params: {
        limit,
        skip,
        search,
      },
    });
  }

  downloadExcelBooks() {
    return this.http.post(this.baseUrl + 'books/book-download', null, {
      responseType: 'blob',
    });
  }

  // Endpoint untuk mendapatkan sebuah data buku berdasarkan kode buku
  getBookByCode(code: string) {
    return this.http.get<any>(this.baseUrl + `books/${code}`);
  }

  // Endpoint untuk menambahkan sebuah data buku baru
  storeBook(data: any) {
    return this.http.post<any>(this.baseUrl + 'books', data);
  }

  // Endpoint untuk menambah data stock baru dengen method PATCH
  updateStockBookByCode(code: string, newStock: number) {
    return this.http.patch<any>(this.baseUrl + `books/${code}`, {
      code,
      stock: newStock,
    });
  }

  // Menghapus buku berdasarkan kode buku
  deleteBookByCode(code: string) {
    return this.http.delete<any>(this.baseUrl + `books/${code}`);
  }
}
