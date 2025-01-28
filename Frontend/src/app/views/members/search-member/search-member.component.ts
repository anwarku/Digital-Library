import { Member } from './../../../models/member';
import { FormsModule } from '@angular/forms';
import {
  ButtonDirective,
  ColComponent,
  FormControlDirective,
  InputGroupComponent,
  RowComponent,
  TableDirective,
} from '@coreui/angular';
import { Component } from '@angular/core';

@Component({
  selector: 'app-search-member',
  imports: [
    RowComponent,
    ColComponent,
    InputGroupComponent,
    FormsModule,
    FormControlDirective,
    ButtonDirective,
    TableDirective,
  ],
  templateUrl: './search-member.component.html',
  styleUrl: './search-member.component.scss',
})
export class SearchMemberComponent {
  memberId: string;
  memberData: Member;
  isMemberFound: boolean = false;

  // Tabel member tambahkan created at

  onSearchMember() {}
}
