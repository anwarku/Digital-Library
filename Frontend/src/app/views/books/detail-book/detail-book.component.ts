import { Component, OnInit } from '@angular/core';
import {
  ButtonDirective,
  ColComponent,
  ListGroupDirective,
  ListGroupItemDirective,
  RowComponent,
} from '@coreui/angular';
import { BookService } from '../../../services/book.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Book } from '../../../models/book';

@Component({
  selector: 'app-detail-book',
  imports: [
    RowComponent,
    ColComponent,
    ListGroupDirective,
    ListGroupItemDirective,
    ButtonDirective,
    RouterLink,
  ],
  templateUrl: './detail-book.component.html',
  styleUrl: './detail-book.component.scss',
})
export class DetailBookComponent implements OnInit {
  book: Book | undefined;
  constructor(
    private route: ActivatedRoute,
    private bookService: BookService
  ) {}

  ngOnInit(): void {
    this.getDetailBook(this.route.snapshot.params['bookCode']);
  }

  getDetailBook(code: string) {
    this.bookService.getBookByCode(code).subscribe(
      (res: any) => {
        this.book = res;
      },
      (err) => {
        console.error(err);
      }
    );
  }
}
