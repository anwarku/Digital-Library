import { GlobalService } from './../../../services/global.service';
import { Component, OnInit } from '@angular/core';
import {
  ButtonDirective,
  ColComponent,
  FormCheckInputDirective,
  FormControlDirective,
  InputGroupComponent,
  InputGroupTextDirective,
  RowComponent,
  TableDirective,
} from '@coreui/angular';
import { FormsModule } from '@angular/forms';
import { MemberService } from '../../../services/member.service';
import { BookService } from '../../../services/book.service';
import { Book } from '../../../models/book';
import { TransactionService } from '../../../services/transaction.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-transaction',
  imports: [
    RowComponent,
    ColComponent,
    FormsModule,
    InputGroupComponent,
    ButtonDirective,
    FormControlDirective,
    TableDirective,
    FormCheckInputDirective,
    InputGroupTextDirective,
  ],
  templateUrl: './create-transaction.component.html',
  styleUrl: './create-transaction.component.scss',
})
export class CreateTransactionComponent implements OnInit {
  memberName: string = '';
  memberPhone: string = '';
  memberId: string = '';
  isValid: boolean = false;
  selectedBook: string[] = [];
  selectedBookTitle: string[] = [];
  allBooks: Book[] = [];
  searchKeyword: string = '';

  constructor(
    private router: Router,
    private memberService: MemberService,
    private globalService: GlobalService,
    private bookService: BookService,
    private transactionService: TransactionService
  ) {}

  ngOnInit(): void {
    this.getBooks();
  }

  onSubmit() {
    // Mengecek bahwa minimal peminjaman buku adalah 1
    if (this.selectedBook.length < 1 || this.selectedBook.length > 3) {
      this.globalService.sweetAlert.fire({
        icon: 'error',
        title: 'Peminjaman buku tidak valid!',
      });
    }
    // Jika sudah valid, maka akan melakukan request ke server untuk menambahkan transaksi baru
    else {
      const data = {
        memberId: this.memberId,
        books: this.selectedBook,
      };

      this.transactionService.addNewTransaction(data).subscribe(
        // Jika mengembalikan HTTP Response Success
        (res: any) => {
          // Melakukan redirect ke halaman borrowed transactions
          // Sambil mengirimkan pesan
          this.router.navigate(['/transactions', 'borrowed'], {
            state: {
              message: 'Add new transaction has been succeed',
            },
          });
        },
        // Jika mengembalikan HTTP Response Error
        (err: any) => {
          // Mengembalikan alert
          this.globalService.sweetAlert.fire({
            icon: 'error',
            title: 'Failed to add new transaction, please contact IT',
          });
        }
      );
    }
  }

  onCheckMember() {
    // Validasi, cek input member id adalah number
    const isNumber = Number(this.memberId);
    if (isNumber) {
      this.memberService.checkMemberById(isNumber).subscribe(
        // Jika HTTP Response Success
        (res: any) => {
          // Lakukan sesuatu ketika berhasil
          this.memberName = res.name;
          this.memberPhone = res.phone;
          this.isValid = true;
        },
        (err: any) => {
          // Lakukan sesuatu ketika error
          this.globalService.sweetAlert.fire({
            icon: 'error',
            title: err.error.message,
          });
          this.memberId = '';
        }
      );
    } else {
      this.globalService.sweetAlert.fire({
        icon: 'error',
        title: 'Member ID harus berupa angka',
      });
      this.memberId = '';
    }
  }

  onSearch() {
    this.getBooks(5, 1, this.searchKeyword);
  }

  onChangeSelectBook(event: any, bookTitle: string) {
    let selectBook = event.target.value;

    if (event.target.checked) {
      // Jika kurang dari sama dengan 3
      if (this.selectedBook.length < 3) {
        this.selectedBook.push(selectBook);
        this.selectedBookTitle.push(bookTitle);
      } else {
        event.target.checked = false;
        this.globalService.sweetAlert.fire({
          icon: 'error',
          title: 'Maksimal peminjaman buku adalah 3 buku',
        });
      }
    } else {
      this.selectedBook = this.selectedBook.filter(
        (book) => book !== selectBook
      );
      this.selectedBookTitle = this.selectedBookTitle.filter(
        (title) => title !== bookTitle
      );
    }
  }

  getBooks(limit: number = 5, page: number = 1, search: string = '') {
    this.bookService
      .getAllBooks(limit, limit * (page - 1), search)
      .subscribe((res: any) => {
        this.allBooks = res.data;
      });
  }
}
