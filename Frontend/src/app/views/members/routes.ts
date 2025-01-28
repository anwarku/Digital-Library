import { CreateMemberComponent } from './create-member/create-member.component';
import { SearchMemberComponent } from './search-member/search-member.component';
import { Routes } from '@angular/router';

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
    ],
  },
];
