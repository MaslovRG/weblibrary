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
        private SupportingFunctions sup; 

        public BookController(ILogger<BookController> nLogger,
            IBookService nBookService,
            IAuthorService nAuthorService,
            IReaderService nReaderService)
        {
            sup = new SupportingFunctions(); 
            _logger = nLogger;
            bookService = nBookService;
            authorService = nAuthorService;
            readerService = nReaderService; 
        }

        // GET: book?page=1&size=5
        [HttpGet]
        public async Task<ActionResult<PagedList<Book>>> Get(int? page, int? size)
        {
            _logger.LogInformation("Get books"); 
            var books = await bookService.GetBooks();
            return GetPagedList(books, page, size); 
        }

        // GET: book/Name
        [HttpGet("{Name}")]
        public async Task<ActionResult<Book>> Get(string Name)
        {
            _logger.LogInformation($"Get book: {Name}"); 
            var book = await bookService.GetBook(Name);

            if (book == null)
            {
                _logger.LogInformation("Can't find book"); 
                return NotFound();
            }

            return book; 
        }

        // POST: book
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Book book)
        {
            _logger.LogInformation("Add book");
            HttpResponseMessage response = null; 
            if (book != null && book.Author != null)
            {
                response = await authorService.AddAuthor(new Author
                {
                    Name = book.Author
                });
                if (response == null || !response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Can't find or add author");
                    book.Author = null;
                }
            }                
            response = await bookService.AddBook(book);
            return GetResponseResult(response); 
        }

        // DELETE: book/Name
        [HttpDelete("{Name}")]
        public async Task<ActionResult> Delete(string Name)
        {
            _logger.LogInformation($"Delete book: {Name}"); 
            var response = await readerService.DeleteBook(Name);
            if (response == null || !response.IsSuccessStatusCode)
                return StatusCode(500); 
            response = await bookService.DeleteBook(Name);
            return GetResponseResult(response); 
        }          

        // GET: book/author/Name
        [HttpGet("author/{Name}")]
        public async Task<ActionResult<PagedList<Book>>> GetBooksByAuthor(string Name, int? page, int? size)
        {
            _logger.LogInformation($"Get books by author: {Name}");
            var books = await bookService.GetBooksByAuthor(Name);
            return GetPagedList(books, page, size); 
        }

        public ActionResult GetResponseResult(HttpResponseMessage response)
        {
            //var code = (int)response.StatusCode;
            if (response == null || !response.IsSuccessStatusCode)
                return StatusCode(500, "Internal error");
            return Ok();
        }

        public ActionResult<PagedList<T>> GetPagedList<T>(List<T> list, int? page, int? size)
        {
            if (list == null)
                return StatusCode(500, "Empty list");
            ActionResult<PagedList<T>> result = new StatusCodeResult(204);
            if (list.Count != 0)
            {
                if (page != null && page > 0 && size != null && size > 0)
                    result = (PagedList<T>)list.ToPagedList((int)page, (int)size);
                else
                    result = (PagedList<T>)list.ToPagedList(1, list.Count);
            }
            return result;
        }
    }
}
