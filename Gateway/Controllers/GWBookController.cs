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
using PagedList;

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
        public async Task<ActionResult<PagedList<Book>>> Get(int? page, int? size)
        {
            var books = await bookService.GetBooks();
            ActionResult<PagedList<Book>> result = NoContent(); 
            if (books != null)
            {
                if (page != null && page > 0 && size != null && size > 0)                    
                    result = (PagedList<Book>)books.ToPagedList((int)page, (int)size); 
                else
                    result = (PagedList<Book>)books.ToPagedList(1, books.Count);                 
            }
            return result; 
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
    }
}
