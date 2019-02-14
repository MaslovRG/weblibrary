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
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        // ObjectResult<IEnumerable<Book>>
        public ObjectResult Get()
        {
            _logger.LogInformation("Get all books");
            string message; 
            ObjectResult result; 
            try
            {
                if (database.Books.Any())
                {
                    message = "Succesful getting";
                    result = Ok(database.Books);
                    _logger.LogInformation(message);
                }
                else
                {
                    message = "No books found";
                    result = NotFound(message);
                    _logger.LogInformation(message); 
                }
            }
            catch
            {
                message = "Problem with database while getting";
                result = StatusCode(500, message);
                _logger.LogError(message);
            }
            
            return result;
        }

        // GET /Name
        [HttpGet("{Name}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        // ObjectResult<Book>
        public ObjectResult Get(string Name)
        {
            _logger.LogInformation($"Get book with name: {Name}");
            string message; 
            ObjectResult result;
            try
            {
                var list = database.Books.Where(book => book.Name == Name);
                if (list.Any())
                {
                    message = "Succesful getting";
                    result = Ok(list.First());
                    _logger.LogInformation(message);
                }
                else
                {
                    message = "No books with this name found";
                    result = NotFound(message); 
                    _logger.LogInformation(message);
                }
            }
            catch
            {
                message = "Problem with database while getting";
                result = StatusCode(500, message);
                _logger.LogError(message);
            }                      
            return result; 
        }

        // POST /
        [HttpPost]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public ObjectResult Post([FromBody]Book book)
        {
            _logger.LogInformation($"Add book: {book.Name}, {book.Year}, {book.Author}");
            string message; 
            ObjectResult result; 
            try
            {
                var Book = new Book
                    { Name = book.Name, Year = book.Year, Author = book.Author };
                if (!database.Books.Where(x => x.Name == book.Name).Any())
                    database.Books.Add(Book);
                database.SaveChanges();
                message = "Succesful adding"; 
                result = Ok(message);
                _logger.LogInformation(message); 
            }
            catch
            {
                message = "Problem with database while adding";
                result = StatusCode(500, message);
                _logger.LogError(message);                 
            }            
            return result; 
        }

        // DELETE /Name
        [HttpDelete("{Name}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public ObjectResult Delete(string Name)
        {
            _logger.LogInformation($"Delete book with id: {Name}");
            string message; 
            ObjectResult result;
            try
            {
                var deleteList = database.Books.Where(book => book.Name == Name);
                if (deleteList.Any())
                {
                    database.Books.RemoveRange(deleteList);                    
                    database.SaveChanges();
                    message = "Succesful deleting";
                    result = Ok(message);
                    _logger.LogInformation(message);
                }
                else
                {
                    message = "Alreadey deleted or not yet added";
                    result = Ok(message);
                    _logger.LogInformation(message);
                }                
            }
            catch
            {
                message = "Problem with database while deleting";
                result = StatusCode(500, message);
                _logger.LogError(message);
            }

            return result; 
        }

        // GET: /author/{Name}
        [HttpGet("author/{Name}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        // ObjectResult<IEnumerable<Book>>
        public ObjectResult GetBooksByAuthor(string Name)
        {
            _logger.LogInformation($"Get books with author: {Name}");
            string message; 
            ObjectResult result;
            try
            {
                var list = database.Books.Where(book => book.Author == Name);
                if (list.Any())
                {
                    message = "Succesful getting";
                    result = Ok(list.ToList());
                    _logger.LogInformation(message); 
                }
                else
                {
                    message = "No books with this author found";
                    result = NotFound(message);
                    _logger.LogInformation(message);
                }
            }
            catch
            {
                message = "Problem with database while getting";
                result = StatusCode(500, message);
                _logger.LogError(message);
            }
            return result;
        }
    }
}
