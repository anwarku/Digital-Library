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
import { jwtDecode, JwtPayload } from 'jwt-decode';

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
  ],
})
export class LoginComponent implements OnInit {
  isLoading: boolean = false;

  constructor(
    private router: Router,
    private userService: UserService,
    private globalService: GlobalService
  ) {}

  ngOnInit(): void {
    // Mengecek apakah pengguna sudah login
    if (this.userService.isLoggedIn()) {
      this.router.navigate(['/books', 'all-books']);
    }
  }

  formLogin = new FormGroup({
    username: new FormControl(''),
    password: new FormControl(''),
  });

  get password() {
    return this.formLogin.get('password');
  }

  onSubmitLogin() {
    console.log(this.formLogin.value);
    this.isLoading = true;
    this.userService.userLogin(this.formLogin.value).subscribe(
      // Jika mengembalikan http response success
      (res: any) => {
        // Jika berhasil maka decode token
        const token = res.token;
        // const decodedToken = jwtDecode<any>(token);
        const decodedToken = jwtDecode<any>(token);

        localStorage.setItem('token', token);
        localStorage.setItem('name', decodedToken.name);

        this.router.navigate(['/books', 'all-books']);

        // console.log(decodedToken);
        // console.log(decodedToken.exp);

        this.isLoading = false;
      },
      // Jika mengembalikan http response error
      (err: any) => {
        this.globalService.sweetAlert.fire({
          icon: 'error',
          title: 'Invalid username and password',
        });
        this.password?.reset();
        this.isLoading = false;
        // console.log(err.error.message);
      }
    );
  }
}
