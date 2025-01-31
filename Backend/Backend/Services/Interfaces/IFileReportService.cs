using Backend.DTOs;

namespace Backend.Services.Interfaces
{
    public interface IFileReportService
    {
        void DownloadReportPdf(int idFileReport, string typeFile);
        void UploadReportPdf(AddFileReportDto addFileReport, IFormFile pdf);
        void UploadReportXlsx(AddFileReportDto addFileReport,IFormFile xlsx);
        bool FileReportIsExist(int idFileReport);
    }
}
