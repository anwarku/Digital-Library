import { Component, OnInit } from '@angular/core';
import { Transaction } from './../../../models/transaction';
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
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { GlobalService } from '../../../services/global.service';
import { TransactionService } from './../../../services/transaction.service';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-borrowed-transactions',
  // Perkara imports component / directive tidak urut
  // CSS tidak jalan di html nya 😊
  imports: [
    FormsModule,
    RowComponent,
    ColComponent,
    TableDirective,
    ButtonDirective,
    BadgeComponent,
    InputGroupComponent,
    InputGroupTextDirective,
    FormControlDirective,
    FormSelectDirective,
    PaginationComponent,
    PageItemComponent,
    PageLinkDirective,
    RouterLink,
    SpinnerComponent,
    DatePipe,
  ],
  templateUrl: './borrowed-transactions.component.html',
  styleUrl: './borrowed-transactions.component.scss',
})
export class BorrowedTransactionsComponent implements OnInit {
  borrowedTransactions: Transaction[] = [];
  alertMessage: string;
  dataLimit = [5, 10, 20];
  totalBorrowedTransactions: number;
  totalPages: number;
  limit: number;
  page: number;
  searchKeyword: string;
  isLoad: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private transactionService: TransactionService,
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

      this.getBorrowedTransactions(
        this.limit ?? 5,
        params['page'] ?? 1,
        this.searchKeyword ?? ''
      );
    });
    this.isLoad = false;
  }

  onSearch() {
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

    this.getBorrowedTransactions(newLimit);
  }

  getBorrowedTransactions(
    limit: number = 5,
    page: number = 1,
    search: string = ''
  ) {
    this.transactionService
      .getBorrowedTransactions(limit, limit * (page - 1), search)
      .subscribe(
        // Jika HTTP Response mengembalikan success
        (res: any) => {
          this.borrowedTransactions = res.data;
          this.totalBorrowedTransactions = res.total;
          this.totalPages = res.total;
          this.isLoad = false;
        },
        // Jika HTTP Response mengembalikan error
        (err: any) => {
          // Berikan feedback ke user
          this.globalService.sweetAlert.fire({
            icon: 'error',
            title: `${err.errors.message}, silakan hubungi IT`,
          });
          this.isLoad = false;
        }
      );
  }
}
