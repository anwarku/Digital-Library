using Backend.Data;
using Backend.DTOs;
using Backend.Helper;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public class FileReportService : IFileReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FileReportService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _environment = env;
        }

        public List<GetFileReportsDto> GetAllFileReport()
        {
            var data = _context.FileReports.ToList();

            List<GetFileReportsDto> result = new List<GetFileReportsDto>();
            foreach (var report in data) 
            {
                result.Add(new GetFileReportsDto
                {
                    Id = report.Id,
                    Name = _context.Users.FirstOrDefault(u => u.Id == report.UserId).Name,
                    TypeFile = report.TypeFile,
                    ReportDate = report.ReportDate,
                    CreatedAt = report.CreatedAt
                });
            }

            return result;
        }

        public FileReport GetFileReportById(int id)
        {
            return _context.FileReports.FirstOrDefault(fr => fr.Id == id);
        }

        public void UploadReport(AddFileReportDto addFileReport, IFormFile file)
        {
            try
            {
                // Mengecek apakah id user ada di dalam database
                bool userIsExist = _context.Users.Any(u => u.Id == addFileReport.UserId);

                // Jika tidak ada kembalikan error
                if (!userIsExist) 
                {
                    throw new Exception("User is not found!");
                }

                // Mengecek apakah ada file yang dikirim
                if (file == null)
                {
                    throw new Exception("File report is required!");
                }

                var report = new FileReport
                {
                    UserId = addFileReport.UserId,
                    ReportDate = addFileReport.ReportDate
                };

                // Mendapatkan ekstensi file upload
                string extFileUpload = Path.GetExtension(file.FileName);

                // Karena file report hanya menerima file pdf dan xlsx
                // Maka disini melakukan pengecekan
                // Jika file xlsx maka akan menjalankan method khusus untuk menyimpan file xlsx ke server
                // Jika file pdf maka akan menjalankan method khusus untuk menyimpan file pdf ke server
                if (extFileUpload == ".pdf")
                {
                    // Simpan jika file tersebut adalah pdf
                    string fileNameResult = SavePdfLocal(file);
                    report.FileName = fileNameResult;
                    report.TypeFile = "pdf";
                }
                else if (extFileUpload == ".xlsx" || extFileUpload == ".xls")
                {
                    // Simpan jika file tersebut adalah spreadsheet
                    string fileNameResult = SaveXlsxLocal(file);
                    report.FileName = fileNameResult;
                    report.TypeFile = "xlsx";
                }
                else
                {
                    // Jika ekstensi file bukan pdf atau spreadsheet, kasih error ygy
                    throw new Exception("File is not supported!");
                }
                _context.FileReports.Add(report);
                _context.SaveChanges();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteReport(int id)
        {
            // Mengecek apakah data ada dalam database
            var report = GetFileReportById(id);
            // Jika ada, cek apakah file ada di server
            if (report != null)
            {
                // Disini bisa saja kita cek user yang mau hapus
                // Bisa kita decode dari token authorization yang dikirim
                // --- Lakukan Sesuatu ---

                // Cek tipe file report, untuk menentukan direktori path file
                var filePath = 
                    report.TypeFile == "pdf" ?
                    Path.Combine(Path.Combine(_environment.WebRootPath, "pdf"), report.FileName) :
                    Path.Combine(Path.Combine(_environment.WebRootPath, "xlsx"), report.FileName);

                // Jika file report ada di database, maka hapus yagesya
                if (System.IO.File.Exists(filePath)) 
                {
                    System.IO.File.Delete(filePath);
                }

                // Hapus data dari database
                _context.FileReports.Remove(report);
                _context.SaveChanges();
            }
        }

        public bool FileReportIsExist(int idFileReport)
        {
            return _context.FileReports.Any(fr => fr.Id == idFileReport);
        }

        private string SavePdfLocal(IFormFile pdfFile)
        {
            try
            {
                var extFile = Path.GetExtension(pdfFile.FileName);
                var fileName = Guid.NewGuid() + extFile;
                var wwwRootPath = Path.Combine(_environment.WebRootPath, "pdf");
                var pdfPath = Path.Combine(wwwRootPath, fileName);

                // Validasi ekstensi file pdf
                if (extFile != ".pdf")
                {
                    throw new Exception("File is not supported!");
                }

                // Validasi ukuran file
                if (pdfFile.Length > Utility.MegaToByte(2))
                {
                    throw new Exception("File size so larger!");
                }

                // Save pdf file
                using (var stream = new FileStream(pdfPath, FileMode.Create))
                {
                    pdfFile.CopyTo(stream);
                }

                return fileName;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string SaveXlsxLocal(IFormFile xlsxFile)
        {
            try
            {
                var extFile = Path.GetExtension(xlsxFile.FileName);
                var fileName = Guid.NewGuid() + extFile;
                var wwwRootPath = Path.Combine(_environment.WebRootPath, "xlsx");
                var pdfPath = Path.Combine(wwwRootPath, fileName);

                var allowedExtensions = new string[] { ".xlsx", ".xls" };
                // Validasi ekstensi file spreadsheet
                if (!allowedExtensions.Contains(extFile))
                {
                    throw new Exception("File is not supported!");
                }

                // Validasi ukuran file
                if (xlsxFile.Length > Utility.MegaToByte(1))
                {
                    throw new Exception("File size so larger!");
                }

                // Save xlsx file
                using (var stream = new FileStream(pdfPath, FileMode.Create))
                {
                    xlsxFile.CopyTo(stream);
                }

                return fileName;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
