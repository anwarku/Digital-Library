import { Routes } from '@angular/router';
import { CreateTransactionComponent } from './create-transaction/create-transaction.component';
import { BorrowedTransactionsComponent } from './borrowed-transactions/borrowed-transactions.component';
import { DetailTransactionComponent } from './detail-transaction/detail-transaction.component';

export const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Books',
    },
    children: [
      {
        path: '',
        redirectTo: '/transactions/borrowed',
        pathMatch: 'full',
      },
      {
        path: 'borrowed',
        component: BorrowedTransactionsComponent,
        data: {
          title: 'Borrowed Transactions',
        },
      },
      {
        path: 'detail/:idTransaction',
        component: DetailTransactionComponent,
        data: {
          title: 'Detail Transaction',
        },
      },
      {
        path: 'create',
        component: CreateTransactionComponent,
        data: {
          title: 'Create Transaction',
        },
      },
    ],
  },
];
