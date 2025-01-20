using Backend.DTOs;
using Backend.Models;

namespace Backend.Services.Interfaces
{
    public interface IBookService
    {
        List<Book> GetAllBooks(int limit, int skip, string search);
        //List<Book> SearchBooks(string keyword);
        Book GetBookByCode(string code);
        void Add(AddBookDto addBookDto);
        void Update(UpdateBookDto updateBookDto);
        void Delete(string code);
        bool Exist(string code);
        int CountAllBooks();
        int CountSearchBooks(string keyword);
    }
}
