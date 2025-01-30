import { CreateMemberComponent } from './create-member/create-member.component';
import { SearchMemberComponent } from './search-member/search-member.component';
import { Routes } from '@angular/router';
import { PrintMemberComponent } from './print-member/print-member.component';

export const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Members',
    },
    children: [
      {
        path: '',
        redirectTo: '/members/search',
        pathMatch: 'full',
      },
      {
        path: 'search',
        component: SearchMemberComponent,
        data: {
          title: 'Search',
        },
      },
      {
        path: 'create',
        component: CreateMemberComponent,
        data: {
          title: 'New Member',
        },
      },
      {
        path: 'print',
        component: PrintMemberComponent,
        data: {
          title: 'Print Member',
        },
      },
    ],
  },
];
