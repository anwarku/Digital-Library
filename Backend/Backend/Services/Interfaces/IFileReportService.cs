using Backend.DTOs;
using Backend.Models;

namespace Backend.Services.Interfaces
{
    public interface IFileReportService
    {
        List<GetFileReportsDto> GetAllFileReport();
        FileReport GetFileReportById(int id);
        void UploadReport(AddFileReportDto addFileReport, IFormFile pdf);
        void DeleteReport(int id);
        bool FileReportIsExist(int idFileReport);
    }
}
