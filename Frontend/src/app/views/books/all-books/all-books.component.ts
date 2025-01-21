import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
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
import Swal from 'sweetalert2';
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
  Toast: any;
  alertMessage: string;
  dataLimit = [5, 10, 20];
  totalBooks: number;
  totalPages: number;
  limit: number;
  page: number;
  searchKeyword: string;
  isLoad: boolean = false;

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

  onClickDelete(code: string) {
    // method confirm (bawaan JS) mengembalikan boolean
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

  // searchBooks(keyword: string)
  // {
  //   this.bookService.getSearchBooks(keyword)
  //   .subscribe(
  //     (res: any) => {
  //       this.books = res.data
  //       this.totalBooks = res.total
  //     }
  //   )
  // }
}
