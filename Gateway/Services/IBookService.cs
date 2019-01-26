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
        Task<List<Book>> GetBooks();
        Task<Book> GetBook(string Name);
        Task<HttpResponseMessage> AddBook(Book book);
        Task<HttpResponseMessage> DeleteBook(string Name); 
    }
}
