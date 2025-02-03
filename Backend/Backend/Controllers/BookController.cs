using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;

namespace Backend.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ApplicationDbContext _context;

        public BookController(IBookService bookService, ApplicationDbContext context)
        {
            _bookService = bookService;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<Book>> GetAllBooks([FromQuery] int limit = 5, int skip = 0, string search = "")
        {
            var books = _bookService.GetAllBooks(limit, skip, search);

            return Ok(new
            {
                limit, skip, total = (search.IsNullOrEmpty()) ? _bookService.CountAllBooks() : _bookService.CountSearchBooks(search), data = books
            });
        }

        //[HttpGet("search")]
        //public ActionResult<List<Book>> SearchBooks([FromQuery] string keyword)
        //{
        //    var books = _bookService.SearchBooks(keyword);
        //    int countBooks = _bookService.CountSearchBooks(keyword);

        //    return Ok(
        //        new
        //        {
        //            total = countBooks, data = books
        //        }
        //        );
        //}

        [HttpGet("{code}")]
        public ActionResult<Book> GetBookByCode(string code)
        {
            var book = _bookService.GetBookByCode(code);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public IActionResult StoreBook([FromBody] AddBookDto addBookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bookService.Add(addBookDto);

            return Created();
        }

        [HttpPost]
        [Route("book-download")]
        public IActionResult DownloadAllBooks(CancellationToken ct)
        {
            // query data from database
            var data = _context.Books.ToList();

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream)) 
            {
                // Mendefinisikan properties file xlsx
                package.Workbook.Properties.Author = "Khaeril Anwar";
                package.Workbook.Properties.Company = "PT. Mencari Cinta Sejati";
                var worksheet = package.Workbook.Worksheets.Add("Sheet hahaa");
                worksheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            stream.Position = 0;
            string fileDownloadName = "download-hahaa.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName);
        }

        [HttpGet]
        [Route("test")]
        public IActionResult PostTest()
        {
            var data = Request.Headers.Authorization;
            var token = Convert.ToString(data).Split(' ')[1];
            //var decoded = 

            DateTime today = new DateTime();
            today = DateTime.Now;
            var idGeneral = Guid.NewGuid();
            var idString = Guid.NewGuid().ToString();

            return Ok(new {Data = "Hello world"});
        }

        [HttpPatch]
        [Route("{code}")]
        public IActionResult UpdateStockBook( string code, [FromBody] UpdateBookDto updateBookDto)
        {
            // Mengecek apakah kode buku antara param dan body
            if (code != updateBookDto.Code)
            {
                return BadRequest(new {message = "Kode buku tidak sesuai"});
            }

            // Mengecek validasi sesuai dengan rule DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bookService.Update(updateBookDto);
            return NoContent();
        }

        [HttpDelete]
        [Route("{code}")]
        public IActionResult DeleteBook(string code)
        {
            // Mengceke apakah buku ada dalam database
            if (!_bookService.Exist(code))
            {
                return NotFound();
            }

            _bookService.Delete(code);
            return NoContent();
        }
    }
}
