import { Routes } from '@angular/router';
import { AllBooksComponent } from './all-books/all-books.component';
import { CreateBookComponent } from './create-book/create-book.component';
import { DetailBookComponent } from './detail-book/detail-book.component';

export const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Books'
    },
    children: [
      {
        path: '',
        redirectTo: 'books',
        pathMatch: 'full'
      },
      {
        path: 'all-books',
        component: AllBooksComponent,
        data: {
          title: 'All Books'
        }
      },
      {
        path: 'create',
        component: CreateBookComponent,
        data: {
          title: 'Create Book'
        }
      },
      {
        path: 'detail/:bookCode',
        component: DetailBookComponent,
        data: {
          title: 'Create Book'
        }
      },
    ]
  }
];

