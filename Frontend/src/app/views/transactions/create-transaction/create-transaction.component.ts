import { Component } from '@angular/core';
import {
  ButtonDirective,
  ColComponent,
  FormControlDirective,
  FormDirective,
  InputGroupComponent,
  RowComponent,
} from '@coreui/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MemberService } from '../../../services/member.service';

@Component({
  selector: 'app-create-transaction',
  imports: [
    RowComponent,
    ColComponent,
    // ReactiveFormsModule,
    FormsModule,
    // RouterLink,
    // FormDirective,
    InputGroupComponent,
    ButtonDirective,
    FormControlDirective,
  ],
  templateUrl: './create-transaction.component.html',
  styleUrl: './create-transaction.component.scss',
})
export class CreateTransactionComponent {
  memberName: string = '';
  memberPhone: string = '';
  memberId: number;

  constructor(private memberService: MemberService) {}

  onCheckMember() {
    // Validasi, cek input member id adalah number
    const isNumber = Number(this.memberId);
    if (isNumber) {
      this.memberService.getMemberById(isNumber).subscribe(
        // Jika HTTP Response Success
        (res: any) => {
          // Lakukan sesuatu ketika berhasil
        },
        (err: any) => {
          // Lakukan sesuatu ketika error
        }
      );
    }
  }
}
