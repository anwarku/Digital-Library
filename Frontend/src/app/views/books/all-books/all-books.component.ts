import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import {
  ButtonCloseDirective,
  ButtonDirective,
  ColComponent,
  FormControlDirective,
  FormSelectDirective,
  InputGroupComponent,
  InputGroupTextDirective,
  ModalBodyComponent,
  ModalComponent,
  ModalFooterComponent,
  ModalHeaderComponent,
  ModalTitleDirective,
  ModalToggleDirective,
  PageItemComponent,
  PageLinkDirective,
  PaginationComponent,
  RowComponent,
  SpinnerComponent,
  TableDirective,
} from '@coreui/angular';
import { Book } from '../../../models/book';
import { BookService } from '../../../services/book.service';
import { GlobalService } from '../../../services/global.service';

@Component({
  selector: 'app-all-books',
  imports: [
    FormsModule,
    RowComponent,
    ColComponent,
    TableDirective,
    InputGroupComponent,
    InputGroupTextDirective,
    FormControlDirective,
    FormSelectDirective,
    RouterLink,
    PaginationComponent,
    PageItemComponent,
    PageLinkDirective,
    ButtonDirective,
    SpinnerComponent,
    ModalComponent,
    ModalHeaderComponent,
    ModalBodyComponent,
    ModalFooterComponent,
    ModalToggleDirective,
    ModalTitleDirective,
    ButtonCloseDirective,
  ],
  templateUrl: './all-books.component.html',
  styleUrl: './all-books.component.scss',
})
export class AllBooksComponent implements OnInit {
  books: Book[] = [];
  alertMessage: string;
  dataLimit = [5, 10, 20];
  totalBooks: number;
  totalPages: number;
  limit: number;
  page: number;
  searchKeyword: string;
  isLoad: boolean = false;
  newStock: number;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private bookService: BookService,
    private globalService: GlobalService
  ) {
    // Mendapatkan alert message dari komponen lain
    const navigation = router.getCurrentNavigation();
    this.alertMessage = navigation?.extras.state?.['message'];
  }

  ngOnInit(): void {
    // Mengecek apakah ada alert message
    if (this.alertMessage) {
      this.globalService.sweetAlert.fire({
        icon: 'success',
        title: this.alertMessage,
      });
    }

    this.isLoad = true;
    // Digunakan agar user bisa memperbarui data query params langsung di URL
    this.route.queryParams.subscribe((params) => {
      // Berikan nilai default agar value data bisa dikirim
      this.limit = params['limit'] ?? 5;
      this.page = params['page'] ? Number(params['page']) : 1;
      this.searchKeyword = params['search'];

      this.getData(
        this.limit ?? 5,
        params['page'] ?? 1,
        this.searchKeyword ?? ''
      );
    });
    this.isLoad = false;
  }

  onSubmitNewStock(bookCode: string) {
    // Mengecek apakah stock input berupa angka
    // Jika input stock bukan angka
    if (!Number(this.newStock)) {
      // Kembalikan alert error
      this.globalService.sweetAlert.fire({
        icon: 'error',
        title: 'Pastikan semua inputan sudah benar',
      });
      // Jika yang di input stock adalah angka
    } else {
      // Melakukan request http ke backend
      // Untuk melakukan penambahan stock buku
      this.bookService
        .updateStockBookByCode(bookCode, Number(this.newStock))
        .subscribe(
          // Jika mengembalikan http response success
          (res: any) => {
            this.globalService.sweetAlert.fire({
              icon: 'success',
              title: 'Berhasil menambahkan stok baru',
            });

            this.getData();
          },
          // Jika mengembalikan http response error
          (err: any) => {
            this.globalService.sweetAlert.fire({
              icon: 'error',
              title: 'Gagal menambah stok buku, silakam hubungi IT',
            });
          }
        );
    }
  }

  onClickDelete(code: string) {
    // Method confirm (bawaan JS) mengembalikan boolean
    const confirmDelete = confirm('Apakah anda yakin ?');

    // Jika user yakin untuk menghapus data
    if (confirmDelete) {
      // Kirim permintaan HTTP ke backend server
      this.bookService.deleteBookByCode(code).subscribe(
        // Jika http response success
        (res: any) => {
          this.getData();
          this.globalService.sweetAlert.fire({
            icon: 'success',
            title: 'Berhasil hapus data buku',
          });
        },
        // Jika http response error
        (err: any) => {
          this.globalService.sweetAlert.fire({
            icon: 'error',
            title: 'Gagal hapus data buku',
          });
        }
      );
    }
  }

  onSearchData() {
    this.isLoad = true;
    this.router.navigate([], {
      queryParams: {
        search: this.searchKeyword,
      },
    });
    this.isLoad = false;
  }

  onSetLimit(e: any) {
    let newLimit = e.target.value;
    this.router.navigate([], {
      queryParams: {
        limit: newLimit,
        page: this.page,
        search: this.searchKeyword,
      },
    });

    this.getData(newLimit);
  }

  getData(limit: number = 5, page: number = 1, search: string = '') {
    this.bookService
      .getAllBooks(limit, limit * (page - 1), search)
      .subscribe((res: any) => {
        this.books = res.data;
        this.totalBooks = res.total;
        this.totalPages = Math.ceil(this.totalBooks / limit);
        this.isLoad = false;
      });
  }
}
