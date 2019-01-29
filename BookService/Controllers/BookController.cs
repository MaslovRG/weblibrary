using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookService.Models; 

namespace BookService.Controllers
{
    [Route("")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private BooksContext database; 
        private readonly ILogger<BookController> _logger; 

        public BookController(BooksContext nDatabase, ILogger<BookController> nLogger)
        {
            _logger = nLogger;
            database = nDatabase; 
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            _logger.LogInformation("Get all books");
            ActionResult<IEnumerable<Book>> result; 
            try
            {
                if (database.Books.Any())
                    result = database.Books;
                else
                    result = NoContent(); 
            }
            catch
            {
                _logger.LogError("Problem with database while getting");
                result = StatusCode(500); 
            }
            
            return result;
        }

        // GET /?Name=name
        [HttpGet("{Name}")]
        public async Task<ActionResult<Book>> Get(string Name)
        {
            _logger.LogInformation($"Get book with name: {Name}");
            ActionResult<Book> result = BadRequest();
            try
            {
                var list = database.Books.Where(book => book.Name == Name);
                if (list.Any())
                    result = list.First();
            }
            catch
            {
                _logger.LogError("Problem with database while getting");
                result = StatusCode(500); 
            }                      
            return result; 
        }

        // POST 
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Book book)
        {
            _logger.LogInformation($"Add book: {book.Name}, {book.Year}, {book.Author}");
            ActionResult result = Ok(); 
            try
            {
                var Book = new Book { Name = book.Name, Year = book.Year, Author = book.Author };
                database.Books.Add(Book);
                database.SaveChanges();
                _logger.LogInformation("Succesful adding"); 
            }
            catch
            {
                _logger.LogError("Problem with database while adding"); 
                result = StatusCode(500); 
            }            
            return result; 
        }

        // DELETE /Name
        [HttpDelete("{Name}")]
        public async Task<ActionResult> Delete(string Name)
        {
            _logger.LogInformation($"Delete book with id: {Name}");
            ActionResult result = Ok();
            try
            {
                var deleteList = database.Books.Where(book => book.Name == Name);
                if (deleteList.Any())
                    database.Books.RemoveRange(deleteList);
                database.SaveChanges(); 
            }
            catch
            {
                _logger.LogError("Problem with database while deleting");
                result = StatusCode(500); 
            }

            return result; 
        }
    }
}
