import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import {
  ButtonDirective,
  ColComponent,
  FormCheckComponent,
  FormCheckInputDirective,
  FormCheckLabelDirective,
  FormControlDirective,
  FormDirective,
  FormFeedbackComponent,
  FormSelectDirective,
  RowComponent,
} from '@coreui/angular';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RequiredComponent } from './../../../components/required/required.component';
import { MemberService } from '../../../services/member.service';
import { GlobalService } from '../../../services/global.service';

@Component({
  selector: 'app-create-member',
  imports: [
    RowComponent,
    ColComponent,
    FormControlDirective,
    FormDirective,
    FormsModule,
    ReactiveFormsModule,
    RequiredComponent,
    FormFeedbackComponent,
    FormCheckComponent,
    FormCheckInputDirective,
    FormCheckLabelDirective,
    FormSelectDirective,
    ButtonDirective,
  ],
  templateUrl: './create-member.component.html',
  styleUrl: './create-member.component.scss',
})
export class CreateMemberComponent implements OnInit {
  fileImage: File;

  listOfJobs: string[] = [
    'Merchandiser, retail',
    'Mudlogger',
    'Computer games developer',
    'Physiotherapist',
    'Paediatric nurse',
    'Osteopath',
    'Pelajar, Mahasiswa',
    'Clothing/textile technologist',
    'Nurse, mental health',
    'Ceramics designer',
    'Advertising copywriter',
    'Investment analyst',
    'Clothing/textile technologist',
    'IT consultant',
    'Fisheries officer',
    'Jewellery designer',
    'Psychotherapist, dance movement',
    'Chemical engineer',
    'Teacher, adult education',
    'Clinical molecular geneticist',
    'Hydrogeologist',
  ];

  @ViewChild('formFile')
  inputImageMember: ElementRef;

  constructor(
    private memberService: MemberService,
    private globalService: GlobalService
  ) {}

  ngOnInit(): void {}

  newMember = new FormGroup({
    name: new FormControl('', [Validators.required]),
    gender: new FormControl('Male', [Validators.required]),
    phone: new FormControl('', [Validators.required]),
    job: new FormControl('', [Validators.required]),
  });

  get name() {
    return this.newMember.get('name');
  }
  get phone() {
    return this.newMember.get('phone');
  }
  get job() {
    return this.newMember.get('job');
  }

  onChangeImage(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      this.fileImage = file;
      // console.log(file.name);
    }
  }

  onSubmitNewMember() {
    // console.log(this.newMember.value);

    this.memberService
      .storeMember(this.newMember.value, this.fileImage)
      .subscribe(
        // when success
        (res: any) => {
          // do something
          this.globalService.sweetAlert.fire({
            icon: 'success',
            title: 'Member has been created',
          });
          this.newMember.reset();
          this.inputImageMember.nativeElement.value = '';
        },
        // when error
        (err: any) => {
          // do something when error
          this.globalService.sweetAlert.fire({
            icon: 'error',
            title: err.error.message,
          });
        }
      );
  }
}
