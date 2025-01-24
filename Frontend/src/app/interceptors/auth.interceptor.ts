import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { UserService } from '../services/user.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
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
      return next(newReq);
    }
  }
  // ### END

  return next(req);
};
