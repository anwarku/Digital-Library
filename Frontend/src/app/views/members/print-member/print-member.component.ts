import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  ButtonDirective,
  ColComponent,
  RowComponent,
  TableDirective,
} from '@coreui/angular';
import { jsPDF } from 'jspdf';
import html2canvas from 'html2canvas';
import { NgxSpinnerService } from 'ngx-spinner';
import { ExcelService } from '../../../services/excel.service';
import { MemberService } from './../../../services/member.service';
import { GlobalService } from './../../../services/global.service';
import { BookService } from '../../../services/book.service';

@Component({
  selector: 'app-print-member',
  imports: [
    RouterLink,
    ButtonDirective,
    RowComponent,
    ColComponent,
    TableDirective,
  ],
  templateUrl: './print-member.component.html',
  styleUrl: './print-member.component.scss',
})
export class PrintMemberComponent {
  constructor(
    private spinner: NgxSpinnerService,
    private excelService: ExcelService,
    private memberService: MemberService,
    private bookService: BookService,
    private globalService: GlobalService
  ) {}

  downloadExcel() {
    this.spinner.show();

    this.memberService.getAllMembers().subscribe(
      // Success
      (res: any) => {
        this.excelService.generateExcel(res, 'all-members');
        this.spinner.hide();
      },
      // err
      (err: any) => {
        this.globalService.sweetAlert.fire({
          icon: 'error',
          title: 'Has error',
        });
      }
    );
  }

  downloadExcelApi() {
    console.log('download file');
    this.bookService.downloadExcelBooks().subscribe(
      // Success
      (res: Blob) => {
        // console.log('downloaded file');
        // console.log(res);
        var blob = new Blob([res], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        });
        var downloadUrl = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.setAttribute('target', '_blank');
        link.setAttribute('href', downloadUrl);
        link.setAttribute('download', 'test-file.xlsx');
        document.body.appendChild(link);
        link.click();
        link.remove();
      }
    );
  }

  generatePdf() {
    const data = document.getElementById('memberCard') as HTMLElement;
    html2canvas(data, { allowTaint: true, useCORS: true }).then((canvas) => {
      const imgWidth = 90;
      const pageHeight = 55;
      const imgHeight = (canvas.height * imgWidth) / canvas.width;
      const heightLeft = imgHeight;

      const contentDataURL = canvas.toDataURL('image/png');
      const pdf = new jsPDF({
        orientation: 'landscape',
        unit: 'mm',
        format: [90, 55],
      }); // A4 size page of PDF

      let position = 0;
      pdf.addImage(contentDataURL, 'PNG', 0, position, imgWidth, imgHeight);
      pdf.save('dynamicData.pdf'); // Generated PDF
    });
  }
}
