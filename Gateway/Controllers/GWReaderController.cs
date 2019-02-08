using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Gateway.Services;
using Gateway.Models.Readers;
using Gateway.Models.Books;
using Gateway.Models.Authors;
using PagedList; 

namespace Gateway.Controllers
{
    [Route("reader")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly ILogger<ReaderController> _logger;
        private IReaderService readerService;
        private IBookService bookService;
        private IAuthorService authorService; 

        public ReaderController(ILogger<ReaderController> nLogger,
            IReaderService nReaderService,
            IBookService nBookService,
            IAuthorService nAuthorService)
        {
            _logger = nLogger;
            readerService = nReaderService;
            bookService = nBookService;
            authorService = nAuthorService;
        }

        // GET: reader?page=1&size=5
        [HttpGet]
        public async Task<ActionResult<PagedList<Reader>>> Get(int? page, int? size)
        {
            _logger.LogInformation("Get readers"); 
            var readers = await readerService.GetReaders();
            return SupportingFunctions.GetPagedList(readers, page, size); 
        }

        // GET: reader/Nickname
        [HttpGet("{Nickname}")]
        public async Task<ActionResult<Reader>> Get(string Nickname)
        {
            _logger.LogInformation($"Get reader: {Nickname}"); 
            var reader = await readerService.GetReader(Nickname);

            if (reader == null)
            {
                _logger.LogInformation("Can't find reader"); 
                return NotFound();
            }            

            return reader;
        }

        // POST: reader
        [HttpPost("{Nickname}")]
        public async Task<ActionResult> Post(string Nickname)
        {
            _logger.LogInformation($"Add reader: {Nickname}"); 
            var response = await readerService.AddReader(Nickname);
            return SupportingFunctions.GetResponseResult(response);
        }

        // POST: reader/Nickname/book/Name
        [HttpPost("{Nickname}/book/{Name}")]
        public async Task<ActionResult> Post(string Nickname, string Name)
        {
            _logger.LogInformation($"Add book {Name} to reader {Nickname}"); 
            var reader = await readerService.GetReader(Nickname);
            var book = await bookService.GetBook(Name); 
            if (reader != null && book != null)
            {
                _logger.LogInformation("Can't find book or reader"); 
                var response = await readerService.AddBookToReader(Nickname, Name);
                return SupportingFunctions.GetResponseResult(response);
            }
            return NotFound(); 
        }

        // DELETE: reader/Nickname/book/Name
        [HttpDelete("{Nickname}/book/{Name}")]
        public async Task<ActionResult> Delete(string Nickname, string Name)
        {
            _logger.LogInformation($"Delete book {Name} from reader {Nickname}"); 
            var response = await readerService.DeleteBookFromReader(Nickname, Name);
            return SupportingFunctions.GetResponseResult(response); 
        }

        // GET: reader/Nickname/books?page=1&size=5
        [HttpGet("{Nickname}/books")]
        public async Task<ActionResult<PagedList<Book>>> GetBooks(string Nickname, int? page, int? size)
        {
            _logger.LogInformation($"Get reader {Nickname} books information"); 
            var nameList = await readerService.GetReaderBooks(Nickname);
            var books = new List<Book>(); 
            if (nameList != null)
                foreach (var bookName in nameList)
                {
                    var book = await bookService.GetBook(bookName);
                    if (book != null)
                        books.Add(book); 
                }
            return SupportingFunctions.GetPagedList(books, page, size); 
        }

        // GET: reader/Nickname/authors?page=1&size=5
        [HttpGet("Nickname/authors")]
        public async Task<ActionResult<PagedList<Author>>> GetAuthors(string Nickname, int? page, int? size)
        {
            _logger.LogInformation($"Get reader {Nickname} readed authors"); 
            var nameList = await readerService.GetReaderBooks(Nickname);
            var authors = new List<Author>();
            if (nameList != null)
                foreach (var bookName in nameList)
                {
                    var book = await bookService.GetBook(bookName);
                    if (book != null && book.Author != null)
                    {
                        var author = await authorService.GetAuthor(book.Author);
                        if (author != null)
                            authors.Add(author); 
                    }
                }
            return SupportingFunctions.GetPagedList(authors, page, size); 
        }
    }
}
