using Backend.Data;
using Backend.DTOs;
using Backend.Helper;
using Backend.Models;
using Backend.Services.Interfaces;
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

        //public void DownloadReport(int idFileReport, string typeFile)
        //{
        //    try
        //    {
        //        // Mengecek apakah ada id file report di database
        //        bool isReportExist = _context.FileReports.Any(fr => fr.Id == idFileReport);

        //        if (!isReportExist)
        //        {
        //            throw new Exception("File report is not found");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public void UploadReportPdf(AddFileReportDto addFileReport, IFormFile pdf)
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
                if (pdf == null)
                {
                    throw new Exception("File report is required!");
                }

                var reportPdf = new FileReport
                {
                    UserId = addFileReport.UserId,
                    ReportDate = addFileReport.ReportDate
                };

                string fileNameResult = SavePdfLocal(pdf);
                reportPdf.FileName = fileNameResult;
                _context.FileReports.Add(reportPdf);
                _context.SaveChanges();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public void UploadReportXlsx(AddFileReportDto addFileReport, IFormFile xlsx)
        {

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
                var wwwRootPath = Path.Combine(_environment.WebRootPath, "pdfs");
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
    }
}
