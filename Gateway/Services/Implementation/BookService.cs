using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Gateway.Models.Books; 

namespace Gateway.Services.Implementation
{
    public class BookService : Service, IBookService
    {
        public BookService(IConfiguration configuration) : 
            base(configuration.GetSection("ServiceAddresses")["BookService"]) { }

        public async Task<List<Book>> GetBooks()
        {
            var response = await Get("");
            return JsonConvert.DeserializeObject<List<Book>>(await response.Content.ReadAsStringAsync()); 
        }

        public async Task<Book> GetBook(string Name)
        {
            var response = await Get(Name);
            return JsonConvert.DeserializeObject<Book>(await response.Content.ReadAsStringAsync()); 
        }
    }
}
