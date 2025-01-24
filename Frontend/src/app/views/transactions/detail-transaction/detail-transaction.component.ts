import { Component, OnInit } from '@angular/core';
import { TransactionService } from './../../../services/transaction.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { GlobalService } from '../../../services/global.service';
import {
  BadgeComponent,
  ButtonDirective,
  CardBodyComponent,
  CardComponent,
  CardHeaderComponent,
  ColComponent,
  RowComponent,
  TableDirective,
} from '@coreui/angular';
import { DatePipe, NgClass } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-detail-transaction',
  imports: [
    DatePipe,
    NgClass,
    RowComponent,
    ColComponent,
    CardComponent,
    CardHeaderComponent,
    CardBodyComponent,
    TableDirective,
    BadgeComponent,
    ButtonDirective,
    RouterModule,
  ],
  templateUrl: './detail-transaction.component.html',
  styleUrl: './detail-transaction.component.scss',
})
export class DetailTransactionComponent implements OnInit {
  transaction: any;
  isLoad: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private transactionService: TransactionService,
    private globalService: GlobalService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.getDetailTransaction(this.route.snapshot.params['idTransaction']);
  }

  onReturned(idTransaction: string) {
    // Kasih konfirmasi apakah peminjam membawa semua buku
    const confirmReturned = confirm(
      'Apakah member membawa semua buku yang dipinjam ?'
    );

    // Jika sudah fix
    if (confirmReturned) {
      this.spinner.show();
      this.transactionService.updateStatusTransaction(idTransaction).subscribe(
        // Jika HTTP Response Success
        (res: any) => {
          // Sembunyikan spinner loading
          this.spinner.hide();

          // Redirect ke borrowed transactions, sambil mengirim message
          this.router.navigate(['/transactions', 'borrowed'], {
            state: {
              message: 'Berhasil update status transaksi!',
            },
          });
        },
        // Jika HTTP Response Error
        (err: any) => {
          this.spinner.hide();
          // Berikan feedback error
          this.globalService.sweetAlert.fire({
            icon: 'error',
            title: 'Gagal melakukan update, silakan hubungi IT',
          });
        }
      );
    }
  }

  getDetailTransaction(idTransaciton: string) {
    this.spinner.show();
    this.isLoad = true;
    this.transactionService.getTransactionById(idTransaciton).subscribe(
      // Jika mengembalikan HTTP Response Succes
      (res: any) => {
        this.transaction = res;
        this.spinner.hide();
        this.isLoad = false;
      },
      // Jika mengembalikan HTTP Response Error
      (err: any) => {
        // Berikan feedback error
        this.globalService.sweetAlert.fire({
          icon: 'error',
          title: `${err.errors.message}, silakan hubungi IT`,
        });

        this.isLoad = false;
      }
    );
  }
}
