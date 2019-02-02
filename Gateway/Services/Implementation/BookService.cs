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

        public async Task<List<Book>> GetBooks()
        {
            var response = await Get("");
            return await GetObjectOrNullFromJson<List<Book>>(response);
        }

        public async Task<Book> GetBook(string Name)
        {
            var response = await Get(Name);
            return await GetObjectOrNullFromJson<Book>(response);
        }

        public async Task<HttpResponseMessage> AddBook(Book Book)
        {
            return await PostJson("", Book); 
        }

        public async Task<HttpResponseMessage> DeleteBook(string Name)
        {
            return await Delete(Name); 
        }

        public async Task<List<Book>> GetBooksByAuthor(string Author)
        {
            var response = await Get($"author/{Author}");
            return await GetObjectOrNullFromJson<List<Book>>(response);
        }
    }
}
