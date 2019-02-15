using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Gateway.Models.Books; 

namespace Gateway.Services
{
    public interface IBookService
    {
        Task<Result<List<Book>>> GetBooks();
        Task<Result<Book>> GetBook(string Name);
        Task<Result> AddBook(Book book);
        Task<Result> DeleteBook(string Name);
        Task<Result<List<Book>>> GetBooksByAuthor(string Author); 
    }
}
