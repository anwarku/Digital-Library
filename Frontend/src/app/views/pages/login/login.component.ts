import { Component, OnInit } from '@angular/core';
import { IconDirective } from '@coreui/icons-angular';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import {
  ContainerComponent,
  RowComponent,
  ColComponent,
  CardGroupComponent,
  TextColorDirective,
  CardComponent,
  CardBodyComponent,
  FormDirective,
  InputGroupComponent,
  InputGroupTextDirective,
  FormControlDirective,
  ButtonDirective,
} from '@coreui/angular';
import { UserService } from '../../../services/user.service';
import { GlobalService } from './../../../services/global.service';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [
    ContainerComponent,
    RowComponent,
    ColComponent,
    CardGroupComponent,
    TextColorDirective,
    CardComponent,
    CardBodyComponent,
    FormDirective,
    InputGroupComponent,
    InputGroupTextDirective,
    IconDirective,
    FormControlDirective,
    ButtonDirective,
    ReactiveFormsModule,
    NgxSpinnerModule,
  ],
})
export class LoginComponent implements OnInit {
  // Kita definisikan dependensi yang kita butuhkan dalam logic ini
  constructor(
    private router: Router,
    private userService: UserService,
    private globalService: GlobalService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    // Mengecek apakah pengguna sudah login
    if (this.userService.isLoggedIn()) {
      this.router.navigate(['/books', 'all-books']);
    }
  }

  // Mendefinisikan form group untuk inputan credentials
  formLogin = new FormGroup({
    username: new FormControl(''),
    password: new FormControl(''),
  });

  // Melakukan getter dari inputan password
  get password() {
    return this.formLogin.get('password');
  }

  // Fungsi ketika user klik tombol login
  onSubmitLogin() {
    // Menjalankan loading
    this.spinner.show();

    // Melakukan request http untuk login, lalu dapatkan akses token JWT
    this.userService.userLogin(this.formLogin.value).subscribe(
      // Jika mengembalikan http response success
      (res: any) => {
        // Jika berhasil maka decode token
        const token = res.token;
        // const decodedToken = jwtDecode<any>(token);
        const decodedToken = jwtDecode<any>(token);

        // Simpan akses token ke dalam local storage
        localStorage.setItem('token', token);
        localStorage.setItem('name', decodedToken.name);

        // Arahkan user ke halaman utama aplikasi
        this.router.navigate(['/books', 'all-books']);

        this.spinner.hide();

        // --- Opsional !!!!!!
        console.clear(); // Disini saya clear console agar rapi, jika user gagal login
      },
      // Jika mengembalikan http response error
      (err: any) => {
        // Kita berikan feedback kepada user bahwa gagal login
        this.globalService.sweetAlert.fire({
          icon: 'error',
          title: 'Invalid username and password',
        });

        // Kita clear inputan password
        this.password?.reset();

        this.spinner.hide();
      }
    );
  }
}
