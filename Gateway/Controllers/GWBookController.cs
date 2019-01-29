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
using Gateway.Models.Authors;
using Gateway.Models.Readers; 
using PagedList;

namespace Gateway.Controllers
{
    [Route("book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger; 
        private IBookService bookService;
        private IAuthorService authorService;
        private IReaderService readerService;

        public BookController(ILogger<BookController> nLogger,
            IBookService nBookService,
            IAuthorService nAuthorService,
            IReaderService nReaderService)
        {
            _logger = nLogger;
            bookService = nBookService;
            authorService = nAuthorService;
            readerService = nReaderService; 
        }

        // GET: book?page=1&size=5
        [HttpGet]
        public async Task<ActionResult<PagedList<Book>>> Get(int? page, int? size)
        {
            var books = await bookService.GetBooks();
            return SupportingFunctions.GetPagedList(books, page, size); 
        }

        // GET: book/Name
        [HttpGet("{Name}")]
        public async Task<ActionResult<Book>> Get(string Name)
        {
            var book = await bookService.GetBook(Name);

            if (book == null)
                return NotFound();

            return book; 
        }

        // POST: book
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Book book)
        {
            var response = await authorService.AddAuthor(new Author
            {
                Name = book.Author 
            });
            var trueBook = book;
            if (response == null || !response.IsSuccessStatusCode)
                trueBook.Author = null; 
            response = await bookService.AddBook(trueBook);
            return SupportingFunctions.GetResponseResult(response); 
        }

        // DELETE: book/Name
        [HttpDelete("{Name}")]
        public async Task<ActionResult> Delete(string Name)
        {
            var response = await readerService.DeleteBook(Name);
            if (response == null || !response.IsSuccessStatusCode)
                return StatusCode(500); 
            response = await bookService.DeleteBook(Name);
            return SupportingFunctions.GetResponseResult(response); 
        }          

        // GET: book/author/Name
        [HttpGet("author/{Name}")]
        public async Task<ActionResult<PagedList<Book>>> GetAuthor(string Name, int? page, int? size)
        {
            var books = await bookService.GetBooksByAuthor(Name);
            return SupportingFunctions.GetPagedList(books, page, size); 
        }
    }
}
