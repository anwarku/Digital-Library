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
import { MemberService } from '../../../services/member.service';
import { GlobalService } from './../../../services/global.service';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

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
    DatePipe,
  ],
  templateUrl: './search-member.component.html',
  styleUrl: './search-member.component.scss',
})
export class SearchMemberComponent {
  memberId: string;
  memberData: Member;
  isMemberFound: boolean = false;

  constructor(
    private memberService: MemberService,
    private globalService: GlobalService
  ) {}

  // Tabel member tambahkan created at

  onSearchMember() {
    // Mengecek apakah user menginputkan adalah angka
    const isNumber = Number(this.memberId);

    if (isNumber) {
      this.memberService.getMemberById(isNumber).subscribe(
        // Jika mengembalikan HTTP Response Success
        (res: any) => {
          this.memberData = res;
          this.isMemberFound = true;
        },
        // Jika mengembalikan HTTP Response Error
        (err: any) => {
          // Memberikn feedback error
          this.globalService.sweetAlert.fire({
            icon: 'error',
            title: err.error.message,
          });
        }
      );
    } else {
      this.memberId = '';
      this.globalService.sweetAlert.fire({
        icon: 'error',
        title: 'Member ID is invalid',
      });
    }
  }
}
