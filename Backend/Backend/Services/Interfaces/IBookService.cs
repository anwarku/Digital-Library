using Backend.DTOs;
using Backend.Models;

namespace Backend.Services.Interfaces
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
        Book GetBookByCode(string code);
        void Add(AddBookDto addBookDto);
        void Update(UpdateBookDto updateBookDto);
        void Delete(string code);
        bool Exist(string code);
    }
}
