import { Routes } from '@angular/router';
import { AllTransactionComponent } from './all-transaction/all-transaction.component';
import { CreateTransactionComponent } from './create-transaction/create-transaction.component';

export const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Books',
    },
    children: [
      {
        path: '',
        redirectTo: '/transactions/all-transactions',
        pathMatch: 'full',
      },
      {
        path: 'all-transactions',
        component: AllTransactionComponent,
        data: {
          title: 'All Transactions',
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
