using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Gateway.Services;
using Gateway.Models.Readers;
using Gateway.Models.Books;
using Gateway.Models.Authors;
using X.PagedList; 

namespace Gateway.Controllers
{
    [Route("api/reader")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly ILogger<ReaderController> _logger;
        private IReaderService readerService;
        private IBookService bookService;
        private IAuthorService authorService;
        private SupportingFunctions sup; 

        public ReaderController(ILogger<ReaderController> nLogger,
            IReaderService nReaderService,
            IBookService nBookService,
            IAuthorService nAuthorService)
        {
            sup = new SupportingFunctions(); 
            _logger = nLogger;
            readerService = nReaderService;
            bookService = nBookService;
            authorService = nAuthorService;
        }

        // GET: reader?page=1&size=5
        [HttpGet]
        // ObjectResult<PagedList<Reader>>
        public async Task<ObjectResult> Get(int? page, int? size)
        {
            _logger.LogInformation("Get readers"); 
            var response = await readerService.GetReaders();

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error");
                return StatusCode(500, "Internal error");
            }

            if (response.Code != 200)
            {
                _logger.LogInformation("Can't get readers");
                return StatusCode(response.Code, response.Message); 
            }

            return GetPagedList(response.Value, page, size); 
        }

        // GET: reader/Nickname
        [HttpGet("{Nickname}")]
        // ObjectResult<Reader>
        public async Task<ObjectResult> Get(string Nickname)
        {
            _logger.LogInformation($"Get reader: {Nickname}"); 
            var response = await readerService.GetReader(Nickname);

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error");
                return StatusCode(500, "Internal error");
            }

            if (response.Code != 200)
            {
                _logger.LogInformation("Can't get reader"); 
                return StatusCode(response.Code, response.Message);
            }

            _logger.LogInformation("Succesfully get reader"); 
            return Ok(response.Value);
        }

        // POST: reader
        [HttpPost("{Nickname}")]
        public async Task<ObjectResult> Post(string Nickname)
        {
            _logger.LogInformation($"Add reader: {Nickname}"); 
            var response = await readerService.AddReader(Nickname);

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error");
                return StatusCode(500, "Internal error");
            }

            return StatusCode(response.Code, response.Message);
        }

        // POST: reader/Nickname/book/Name
        [HttpPost("{Nickname}/book/{Name}")]
        public async Task<ObjectResult> Post(string Nickname, string Name)
        {
            _logger.LogInformation($"Add book {Name} to reader {Nickname}"); 
            var reader = await readerService.GetReader(Nickname);
            var book = await bookService.GetBook(Name); 

            if (reader != null && reader.Code == 200 && book != null && book.Code == 200)
            {                
                var response = await readerService.AddBookToReader(Nickname, Name);

                if (response == null)
                {
                    _logger.LogInformation("Internal gateway error");
                    return StatusCode(500, "Internal error");
                }

                return StatusCode(response.Code, response.Message);
            }

            _logger.LogInformation("Can't find book or reader");
            return StatusCode(404, "Can't find book or reader"); 
        }

        // DELETE: reader/Nickname/book/Name
        [HttpDelete("{Nickname}/book/{Name}")]
        public async Task<ObjectResult> Delete(string Nickname, string Name)
        {
            _logger.LogInformation($"Delete book {Name} from reader {Nickname}"); 
            var response = await readerService.DeleteBookFromReader(Nickname, Name);

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error");
                return StatusCode(500, "Internal error");
            }

            return StatusCode(response.Code, response.Message); 
        }

        // GET: reader/Nickname/books?page=1&size=5
        [HttpGet("{Nickname}/books")]
        // ObjectResult<PagedList<Book>>
        public async Task<ObjectResult> GetBooks(string Nickname, int? page, int? size)
        {
            _logger.LogInformation($"Get reader {Nickname} books information"); 
            var response = await readerService.GetReaderBooks(Nickname);

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error");
                return StatusCode(500, "Internal error");
            }

            var books = new List<Book>();
            if (response.Code == 200)
            {
                var nameList = response.Value;
                foreach (var bookName in nameList)
                {
                    var bookresponse = await bookService.GetBook(bookName);
                    if (bookresponse != null && bookresponse.Code == 200)
                        books.Add(bookresponse.Value); 
                }
                return GetPagedList(books, page, size);
            }
            return StatusCode(response.Code, response.Message);
        }

        // GET: reader/Nickname/authors?page=1&size=5
        [HttpGet("{Nickname}/authors")]
        // ObjectResult<PagedList<Author>>
        public async Task<ObjectResult> GetAuthors(string Nickname, int? page, int? size)
        {
            _logger.LogInformation($"Get reader {Nickname} readed authors"); 
            var response = await readerService.GetReaderBooks(Nickname);

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error");
                return StatusCode(500, "Internal error");
            }

            var authors = new List<Author>();
            if (response.Code == 200)
            {
                var nameList = response.Value; 
                foreach (var bookName in nameList)
                {
                    var bookresponse = await bookService.GetBook(bookName);
                    if (bookresponse != null && bookresponse.Code == 200 && bookresponse.Value.Author != null)
                    {
                        var authorresponse = await authorService.GetAuthor(bookresponse.Value.Author);
                        if (authorresponse.Code == 200)
                            authors.Add(authorresponse.Value);
                    }
                }
                return GetPagedList(authors, page, size);
            }
            return StatusCode(response.Code, response.Message); 
        }

        public ObjectResult GetPagedList<T>(List<T> list, int? page, int? size)
        {
            ObjectResult result = StatusCode(404, "Not found");
            if (list == null)
            {
                result = StatusCode(500, "Empty list");
                _logger.LogInformation("Empty list");
            }

            if (list != null && list.Count != 0)
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
