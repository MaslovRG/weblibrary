using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Gateway.Services;
using Gateway.Models; 
using Gateway.Models.Readers;
using Gateway.Models.Books;
using Gateway.Models.Authors;

namespace Gateway.Controllers
{
    [Route("api/all")]
    [ApiController]
    public class AllController : ControllerBase
    {
        private readonly ILogger<AllController> _logger;
        private IReaderService readerService;
        private IBookService bookService;
        private IAuthorService authorService;

        public AllController(ILogger<AllController> nLogger,
            IReaderService nReaderService,
            IBookService nBookService,
            IAuthorService nAuthorService)
        {
            _logger = nLogger;
            readerService = nReaderService;
            bookService = nBookService;
            authorService = nAuthorService;
        }

        [HttpGet("statistics")]
        public async Task<ObjectResult> GetStat()
        {
            _logger.LogInformation("Get stats"); 
            Statistics stat = new Statistics();
            var books = await bookService.GetBooks(); 
            if (books.Code == 200)
                stat.BookCount = books.Value.Count;
            if (books.Code == 404)
                stat.BookCount = 0;

            var authors = await authorService.GetAuthors();
            if (authors.Code == 200)
                stat.AuthorCount = authors.Value.Count;
            if (authors.Code == 404)
                stat.AuthorCount = 0;

            var readers = await readerService.GetReaders();
            if (readers.Code == 200)
                stat.ReaderCount = books.Value.Count;
            if (readers.Code == 404)
                stat.ReaderCount = 0;

            return Ok(stat); 
        }
    }
}