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
  selector: 'app-history-transactions',
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
  templateUrl: './history-transactions.component.html',
  styleUrl: './history-transactions.component.scss',
})
export class HistoryTransactionsComponent {
  returnedTransactions: Transaction[] = [];
  dataLimit = [5, 10, 20];
  totalReturnedTransactions: number;
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
  ) {}

  ngOnInit(): void {
    this.isLoad = true;
    // Digunakan agar user bisa memperbarui data query params langsung di URL
    this.route.queryParams.subscribe((params) => {
      // Berikan nilai default agar value data bisa dikirim
      this.limit = params['limit'] ?? 5;
      this.page = params['page'] ? Number(params['page']) : 1;
      this.searchKeyword = params['search'];

      this.getReturnedTransactions(
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

    this.getReturnedTransactions(newLimit);
  }

  getReturnedTransactions(
    limit: number = 5,
    page: number = 1,
    search: string = ''
  ) {
    this.transactionService
      .getReturnedTransactions(limit, limit * (page - 1), search)
      .subscribe(
        // Jika HTTP Response mengembalikan success
        (res: any) => {
          this.returnedTransactions = res.data;
          this.totalReturnedTransactions = res.total;
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
