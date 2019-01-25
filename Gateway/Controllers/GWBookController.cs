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
            List<Book> books = await bookService.GetBooks();
            ActionResult<List<Book>> result = books; 
            return result;
        }

        // GET: book/Name
        [HttpGet("{Name}")]
        public string Get(string Name)
        {
            return "value";
        }

        // POST: api/Book
        [HttpPost]
        public void Post(string Name, int Year)
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
