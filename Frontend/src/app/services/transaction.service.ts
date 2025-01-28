import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) {}

  getBorrowedTransactions(limit: number, skip: number, search: string) {
    return this.http.get<any>(this.baseUrl + 'transactions/borrowed', {
      params: {
        limit,
        skip,
        search,
      },
    });
  }

  getReturnedTransactions(limit: number, skip: number, search: string) {
    return this.http.get<any>(this.baseUrl + 'transactions/returned', {
      params: {
        limit,
        skip,
        search,
      },
    });
  }

  getTransactionById(idTransaction: string) {
    return this.http.get<any>(this.baseUrl + `transactions/${idTransaction}`);
  }

  updateStatusTransaction(idTransaction: string) {
    return this.http.patch<any>(
      this.baseUrl + `transactions/${idTransaction}`,
      { id: idTransaction }
    );
  }

  addNewTransaction(data: any) {
    return this.http.post<any>(this.baseUrl + 'transactions', data);
  }
}
