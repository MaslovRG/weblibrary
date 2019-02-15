using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Gateway.Models.Books;
using Gateway.Services;

namespace Gateway.Services.Implementation
{
    public class BookService : Service, IBookService
    {
        public BookService(IConfiguration configuration) : 
            base(configuration.GetSection("ServiceAddresses")["BookService"]) { }

        public async Task<Result<List<Book>>> GetBooks()
        {
            var response = await Get("");
            return await Result<List<Book>>.CreateAsync(response);
        }

        public async Task<Result<Book>> GetBook(string Name)
        {
            var response = await Get(Name);
            return await Result<Book>.CreateAsync(response);
        }

        public async Task<Result> AddBook(Book Book)
        {
            var response = await PostJson("", Book);
            return await Result.CreateAsync(response); 
        }

        public async Task<Result> DeleteBook(string Name)
        {
            var response = await Delete(Name);
            return await Result.CreateAsync(response); 
        }

        public async Task<Result<List<Book>>> GetBooksByAuthor(string Author)
        {
            var response = await Get($"author/{Author}");
            return await Result<List<Book>>.CreateAsync(response);
        }
    }
}
