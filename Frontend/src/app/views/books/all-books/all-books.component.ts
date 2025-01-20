import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import {
  BadgeComponent,
  ButtonDirective,
  ColComponent,
  FormControlDirective,
  FormSelectDirective,
  InputGroupComponent,
  InputGroupTextDirective,
  PageItemComponent,
  PageLinkDirective,
  PaginationComponent,
  RowComponent,
  SpinnerComponent,
  TableDirective,
} from '@coreui/angular';
import { Book } from '../../../models/book';
import { BookService } from '../../../services/book.service';

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
  ],
  templateUrl: './all-books.component.html',
  styleUrl: './all-books.component.scss',
})
export class AllBooksComponent implements OnInit {
  books: Book[] = [];
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
    private bookService: BookService
  ) {}

  ngOnInit(): void {
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
  }

  onSearchData() {
    this.isLoad = true;
    this.getData(5, 0, this.searchKeyword);
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

  getData(limit: number = 5, page: number = 0, search: string = '') {
    this.bookService
      .getAllBooks(limit, limit * (page - 1), search)
      .subscribe((res: any) => {
        this.books = res.data;
        this.totalBooks = res.total;
        this.totalPages = Math.ceil(this.totalBooks / limit);

        if (search.length !== 0) {
          this.router.navigate([], {
            queryParams: {
              search: this.searchKeyword,
            },
          });
        }
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
