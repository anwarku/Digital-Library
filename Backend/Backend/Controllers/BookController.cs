using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
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
        [Route("test")]
        public IActionResult PostTest([FromBody] AddBookDto addBookDto)
        {
            return Ok(addBookDto);
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
