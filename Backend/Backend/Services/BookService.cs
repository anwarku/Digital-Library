using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public Book GetBookByCode(string code)
        {
            return _context.Books.FirstOrDefault(b => b.Code == code);
        }

        public void Add(AddBookDto addBookDto)
        {
            var book = new Book
            {
                Code = GenerateBookCode(),
                Title = addBookDto.Title,
                Author = addBookDto.Author,
                Publisher = addBookDto.Publisher,
                PublishYear = addBookDto.PublishYear,
                Isbn = addBookDto.Isbn,
                Stock = addBookDto.Stock
            };
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Update(UpdateBookDto updateBookDto)
        {
            var existBook = GetBookByCode(updateBookDto.Code);
            if (existBook != null)
            {
                existBook.Stock += updateBookDto.Stock;
                _context.SaveChanges();
            }
        }

        public void Delete(string code)
        {
            var existBook = GetBookByCode(code);
            if (existBook != null)
            {
                _context.Books.Remove(existBook);
                _context.SaveChanges();
            }
        }

        public bool Exist(string code)
        {
            return _context.Books.Any(b => b.Code == code);
        }

        private string GenerateBookCode()
        {
            DateTime now = DateTime.Now;
            string randomNumber = new Random().Next(1, 9999).ToString("D4");
            string bookCode = $"BK-{now.Year}-{randomNumber}";
            return bookCode;
        }
    }
}
