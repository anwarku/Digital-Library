import { Component, OnInit } from '@angular/core';
import {
  ButtonCloseDirective,
  ButtonDirective,
  ColComponent,
  ModalBodyComponent,
  ModalComponent,
  ModalFooterComponent,
  ModalHeaderComponent,
  ModalTitleDirective,
  ModalToggleDirective,
  RowComponent,
  TableDirective,
} from '@coreui/angular';
import { ReportService } from './../../services/report.service';
import { Report } from './../../models/report';
import { DatePipe, NgIf } from '@angular/common';
import { GlobalService } from '../../services/global.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-reports',
  imports: [
    RowComponent,
    ColComponent,
    ButtonDirective,
    TableDirective,
    DatePipe,
    ModalHeaderComponent,
    ModalBodyComponent,
    ModalFooterComponent,
    ModalComponent,
    ModalTitleDirective,
    ButtonCloseDirective,
    FormsModule,
    NgIf,
  ],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.scss',
})
export class ReportsComponent implements OnInit {
  reports: Report[] = [];
  isVisible: boolean = false;
  fileReport: File;
  dateReport: string;
  dateError: boolean = false;
  fileError: boolean = false;
  haveError: boolean = false;

  constructor(
    private reportService: ReportService,
    private globalService: GlobalService
  ) {}

  ngOnInit(): void {
    this.getAllReport();
  }

  toggleReportModal() {
    this.isVisible = !this.isVisible;
  }

  onChangeFile(e: any) {
    const file: File = e.target.files[0];

    if (file) {
      this.fileReport = file;
    }
  }

  onUploadReport() {
    // cek apakah date report sudah diisi
    if (
      this.dateReport == undefined ||
      this.dateReport == '' ||
      this.dateReport == null
    ) {
      this.dateError = true;
      this.haveError = true;
    } else {
      this.dateError = false;
      this.haveError = false;
    }

    // cek apakah file sudah dipilih
    if (this.fileReport == null || this.fileReport == undefined) {
      this.fileError = true;
      this.haveError = true;
    } else {
      this.fileError = false;
      this.haveError = false;
    }

    // Jika sudah tidak ada error, maka aplod file itu
    if (!this.haveError) {
      this.reportService
        .uploadReport(this.dateReport, this.fileReport)
        .subscribe(
          // Success
          (res: any) => {
            // Do something
            this.isVisible = false;
            this.globalService.sweetAlert.fire({
              icon: 'success',
              title: 'File has been uploaded!',
            });
          },
          // Error
          (err: any) => {
            // Do something
            this.globalService.sweetAlert.fire({
              icon: 'error',
              title: err.error.message,
            });
          }
        );
    }
  }

  handleVisibleChange(e: any) {
    this.isVisible = e;
  }

  onDownloadReport(id: number, type: string) {
    this.reportService.downloadReportById(id).subscribe(
      // Success
      (res: Blob) => {
        let blob = new Blob([res]);
        let downloadUrl = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.setAttribute('target', '_blank');
        link.setAttribute('href', downloadUrl);
        link.setAttribute('download', `report.${type}`);
        document.body.appendChild(link);
        link.click();
        link.remove();
      }
      // Error
    );
  }

  getAllReport() {
    this.reportService.getAllReport().subscribe(
      // Success
      (res: any) => {
        this.reports = res;
      },
      // Error
      (err: any) => {
        this.globalService.sweetAlert.fire({
          icon: 'error',
          title: 'Failed to get data from server',
        });
      }
    );
  }
}
