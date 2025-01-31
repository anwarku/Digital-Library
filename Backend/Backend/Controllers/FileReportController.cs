using Backend.DTOs;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class FileReportController : ControllerBase
    {
        private readonly IFileReportService _fileReportService;

        public FileReportController(IFileReportService fileReportService)
        {
            _fileReportService = fileReportService;
        }

        [HttpPost]
        public IActionResult DownloadFile([FromBody] int idFileReport, string typeFile)
        {
            try
            {
                // Mengecek apakah file report terdata di database
                bool reportIsExist = _fileReportService.FileReportIsExist(idFileReport);
                if (!reportIsExist)
                {
                    throw new Exception("File report is not found!");
                }

                // Mengecek apakah file tersebut ada dalam server
                var file = System.IO.File

                return File();
            }
            catch (Exception ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpPost]
        public IActionResult UploadReportPdf([FromForm] AddFileReportDto fileReportDto, IFormFile pdf)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }

                _fileReportService.UploadReportPdf(fileReportDto, pdf);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }
    }
}
