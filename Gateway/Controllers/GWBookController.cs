using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult<List<Book>>> Get()
        {
            return await bookService.GetBooks();
        }

        // GET: book/Name
        [HttpGet("{Name}")]
        public async Task<ActionResult<Book>> Get(string Name)
        {
            return await bookService.GetBook(Name);
        }

        // POST: api/Book
        [HttpPost]
        public void Post(string Name, int Year, string Author)
        {

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

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
