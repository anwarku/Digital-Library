import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  ButtonDirective,
  ColComponent,
  FormControlDirective,
  FormDirective,
  FormLabelDirective,
  RowComponent,
} from '@coreui/angular';
import { RequiredComponent } from './../../../components/required/required.component';
import { BookService } from '../../../services/book.service';
import { ActivatedRoute, Router } from '@angular/router';
import { GlobalService } from '../../../services/global.service';
import { NgIf } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { CustomValidator } from '../../../shared/validation';

@Component({
  selector: 'app-create-book',
  imports: [
    NgIf,
    RowComponent,
    ColComponent,
    FormDirective,
    FormControlDirective,
    FormLabelDirective,
    FormsModule,
    ReactiveFormsModule,
    RequiredComponent,
    ButtonDirective,
  ],
  templateUrl: './create-book.component.html',
  styleUrl: './create-book.component.scss',
})
export class CreateBookComponent implements OnInit {
  newBookForm!: FormGroup;
  hasError: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private bookService: BookService,
    private globalService: GlobalService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    // Mendefinisikan form group
    this.newBookForm = new FormGroup({
      title: new FormControl('', [
        Validators.required,
        Validators.minLength(10),
      ]),
      author: new FormControl('', [Validators.required]),
      publisher: new FormControl(''),
      publishYear: new FormControl('', [
        Validators.required,
        Validators.pattern('^(200[9-9]|20[1-2][0-9]|202[0-5])$'),
      ]),
      isbn: new FormControl(''),
      stock: new FormControl('', [
        Validators.required,
        CustomValidator.numberValidator,
      ]),
    });
  }

  get title() {
    return this.newBookForm.get('title');
  }
  get author() {
    return this.newBookForm.get('author');
  }
  get publisher() {
    return this.newBookForm.get('publisher');
  }
  get publishYear() {
    return this.newBookForm.get('publishYear');
  }
  get isbn() {
    return this.newBookForm.get('isbn');
  }
  get stock() {
    return this.newBookForm.get('stock');
  }

  // Function untuk handling tambah data
  onSubmit() {
    this.spinner.show();
    // Mengecek apakah validasi sudah sukses
    // Jika belum valid, kasih alert
    if (!this.newBookForm.valid) {
      this.spinner.hide();
      this.globalService.sweetAlert.fire({
        icon: 'warning',
        title: 'Pastikan semua inputan sudah benar!',
      });
      this.hasError = true;
    }
    // Jika sudah valid, kirim request post ke backend
    else {
      this.hasError = false;
      // Bersihkan data, terutama convert year dan stock ke number
      const data = this.newBookForm.value;

      // Kita replace stock dan publishYear jadi number
      // Karena server backend menerima field tersebut sebagai integer
      data['stock'] = Number(data['stock']);
      data['publishYear'] = Number(data['publishYear']);

      // Kirim permintaan HTTP ke backend POST
      this.bookService.storeBook(data).subscribe(
        // Ketika success response
        (res: any) => {
          this.spinner.hide();
          this.router.navigate(['/books', 'all-books'], {
            state: {
              message: 'Berhasil menambahkan buku baru',
            },
          });
        },
        // Ketika error response
        (err: any) => {
          this.spinner.hide();

          this.globalService.sweetAlert.fire({
            icon: 'error',
            title: 'Gagal menambahkan data buku!',
          });
        }
      );
    }
  }
}
