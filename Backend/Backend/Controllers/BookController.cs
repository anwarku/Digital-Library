﻿using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<List<Book>> GetAllBooks([FromQuery] string limit, string skip, string search)
        {
            var books = _bookService.GetAllBooks();
            return Ok(books);
        }

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
