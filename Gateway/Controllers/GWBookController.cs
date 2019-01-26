using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using Gateway.Services;
using Gateway.Models.Books; 

namespace Gateway.Controllers
{
    [Route("book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger; 
        private IBookService bookService;

        public BookController(ILogger<BookController> nLogger,
            IBookService nBookService)
        {
            _logger = nLogger;
            bookService = nBookService; 
        }

        // GET: book
        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get(int? page, int? size)
        {
            return await bookService.GetBooks();
            /*var paginatedList = new Pa
            return null;*/ 
        }

        // GET: book/Name
        [HttpGet("{Name}")]
        public async Task<ActionResult<Book>> Get(string Name)
        {
            return await bookService.GetBook(Name);
        }

        // POST: api/Book
        [HttpPost]
        public async void Post([FromBody] Book book)
        {
            await bookService.AddBook(book); 
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{Name}")]
        public async void Delete(string Name)
        {
            await bookService.DeleteBook(Name);  
        }
        
        /*
        // PUT: api/Book/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        */        
    }
}
