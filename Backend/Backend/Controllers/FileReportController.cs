using System.Globalization;
using Backend.DTOs;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class FileReportController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IFileReportService _fileReportService;
        private readonly IUserService _userService;

        public FileReportController(IWebHostEnvironment environment, IFileReportService fileReportService, IUserService userService)
        {
            _fileReportService = fileReportService;
            _environment = environment;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<GetFileReportsDto>> GetAllFileReport()
        {
            try
            {
                var allReport = _fileReportService.GetAllFileReport();

                return Ok(allReport);
            }
            catch (Exception ex) 
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpGet]
        [Route("download/{idFileReport}")]
        public IActionResult DownloadFile(int idFileReport)
        {
            try
            {
                // Mengecek apakah file report terdata di database
                var fileReport = _fileReportService.GetFileReportById(idFileReport) ?? throw new Exception("File report is not found!");

                // Mengecek tipe file dari report file
                var wwwRootPath = 
                    fileReport.TypeFile == "pdf" ?
                    Path.Combine(_environment.WebRootPath, "pdf") :
                    Path.Combine(_environment.WebRootPath, "xlsx");

                // Mengecek apakah file tersebut ada dalam server
                var filePath = Path.Combine(wwwRootPath, fileReport.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { Message = "File is not found on server!" });
                }

                var userReported = _userService.GetUserById(fileReport.UserId);
                var dateReportFormat = fileReport.ReportDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                var fileNameResult = $"Report_{userReported.Name}_{dateReportFormat}.{fileReport.TypeFile}";
                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                return File(fileBytes, "application/octet-stream", fileNameResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpPost]
        [Route("upload")]
        public IActionResult UploadReport([FromForm] AddFileReportDto fileReportDto, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }

                _fileReportService.UploadReport(fileReportDto, file);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteReport(int id)
        {
            try
            {
                _fileReportService.DeleteReport(id);
                return NoContent();
            }
            catch (Exception ex) 
            {
                return BadRequest(new {Message = ex.Message});
            }
        }
    }
}
