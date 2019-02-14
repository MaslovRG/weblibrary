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
            var response = await bookService.GetBooks();

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error");
                return StatusCode(500, "Internal error");
            }

            if (response.Code != 200)
            {
                _logger.LogInformation("Can't get books"); 
                return StatusCode(response.Code, response.Message); 
            }

            return GetPagedList(response.Value, page, size); 
        }

        // GET: book/Name
        [HttpGet("{Name}")]
        public async Task<ActionResult<Book>> Get(string Name)
        {
            _logger.LogInformation($"Get book: {Name}"); 
            var response = await bookService.GetBook(Name);

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error");
                return StatusCode(500, "Internal error");
            }

            if (response.Code != 200)
            {
                _logger.LogInformation("Can't get book"); 
                return StatusCode(response.Code, response.Message);
            }

            _logger.LogInformation("Succesfully get book"); 
            return Ok(response.Value); 
        }

        // POST: book
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Book book)
        {
            _logger.LogInformation("Add book");
            Result response = null; 
            if (book != null && book.Author != null)
            {
                response = await authorService.AddAuthor(new Author
                {
                    Name = book.Author
                });
                if (response == null || response.Code != 200)
                {
                    _logger.LogInformation("Can't find or add author");
                    book.Author = null;
                }
            }                
            response = await bookService.AddBook(book);

            if (response == null)
            {
                return StatusCode(500, "Internal error");
            }

            return StatusCode(response.Code, response.Message); 
        }

        // DELETE: book/Name
        [HttpDelete("{Name}")]
        public async Task<ActionResult> Delete(string Name)
        {
            _logger.LogInformation($"Delete book: {Name}"); 
            var response = await readerService.DeleteBook(Name);

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error");
                return StatusCode(500, "Internal error");
            }

            if (response.Code != 200)
            {
                _logger.LogInformation("Can't delete book from readers"); 
                return StatusCode(response.Code, response.Message);
            }

            response = await bookService.DeleteBook(Name);
            return StatusCode(response.Code, response.Message); 
        }          

        // GET: book/author/Name
        [HttpGet("author/{Name}")]
        public async Task<ActionResult<PagedList<Book>>> GetBooksByAuthor(string Name, int? page, int? size)
        {
            _logger.LogInformation($"Get books by author: {Name}");
            var response = await bookService.GetBooksByAuthor(Name);

            if (response == null)
            {
                return StatusCode(500, "Internal error");
            }

            if (response.Code != 200)
            {
                _logger.LogInformation("Can't get books by author"); 
                return StatusCode(response.Code, response.Message);
            }

            return GetPagedList(response.Value, page, size); 
        }       

        public ActionResult<PagedList<T>> GetPagedList<T>(List<T> list, int? page, int? size)
        {
            if (list == null)
                return StatusCode(500, "Empty list");
            ActionResult<PagedList<T>> result = new StatusCodeResult(204);
            if (list.Count != 0)
            {
                if (page != null && page > 0 && size != null && size > 0)
                    result = Ok((PagedList<T>)list.ToPagedList((int)page, (int)size));
                else
                    result = Ok((PagedList<T>)list.ToPagedList(1, list.Count));
                _logger.LogInformation("Succesfully get list"); 
            }
            return result;
        }
    }
}
