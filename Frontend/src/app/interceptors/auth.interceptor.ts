import {
  HttpErrorResponse,
  HttpEvent,
  HttpEventType,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { UserService } from '../services/user.service';
import { catchError, Observable, tap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> => {
  // Import service yang dibutuhkan
  const userService = inject(UserService);

  // Mendapatkan akses token dari local storage
  const token = localStorage.getItem('token') as string;

  // ------- !!! DISCLAIMER !!! --------
  // Algoritma ini di pakai jika akses token cuma sekali, yaitu saat login
  // ### START
  // Mengecek apakah ada akses token dalam local storage
  if (token) {
    // Mengecek apakah token sudah kadaluarsa
    let expToken = jwtDecode<JwtPayload>(token).exp;
    expToken = expToken ? expToken * 1000 : 0; // ini kita konversi nilainya agar sama dengan Date.now()

    // Jika akses token sudah kadaluarsa
    if (Date.now() >= expToken) {
      // Maka melakukan logout dan redirect ke halaman login
      userService.userLogout();
    }

    // Jika akses token belum kadaluarsa
    // Maka kirimkan akses token dalam header
    else {
      // Clone request header untuk bisa ditambahkan header Authorization
      const newReq = req.clone({
        headers: req.headers.append('Authorization', `Bearer ${token}`),
      });

      // Dan kembalikan request header yang sudah di clone
      // Yang berisikan token dalam header
      return next(newReq)
        .pipe
        // tap((event) => {
        //   if (event.type === HttpEventType.Response) {
        //     console.log('ini intercept response');
        //   }
        // }),
        // catchError((err: HttpErrorResponse) => {
        //   if (err.status === 401) {
        //     userService.userLogout();
        //   }
        //   return throwError(err.error.message);
        // })
        ();
    }
  }
  // ### END

  return next(req);
};
