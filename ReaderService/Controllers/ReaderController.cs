using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReaderService.Models; 

namespace ReaderService.Controllers
{
    [Route("")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private ReadersContext database;
        private readonly ILogger<ReaderController> _logger; 

        public ReaderController(ReadersContext nDatabase,
            ILogger<ReaderController> nLogger)
        {
            database = nDatabase;
            _logger = nLogger; 
        }

        // GET /
        [HttpGet]
        public ActionResult<IEnumerable<ReaderOutput>> Get()
        {
            _logger.LogInformation("Get all readers");
            ActionResult<IEnumerable<ReaderOutput>> result;
            try
            {
                var readers = new List<ReaderOutput>(); 
                if (database.Readers.Any())
                {
                    foreach (var reader in database.Readers)
                    {
                        var books = database.ReadedBooks.Where(x => x.Reader.Nickname == reader.Nickname).Select(x => x.Name).ToList(); 
                        var nickname = reader.Nickname;
                        var addedreader = new ReaderOutput()
                        {
                            Nickname = nickname,
                            Books = books
                        };
                        readers.Add(addedreader); 
                    }
                    result = readers; 
                    _logger.LogInformation("Successful getting");
                }
                else
                {
                    result = NoContent();
                    _logger.LogInformation("Empty database"); 
                }
            }
            catch
            {
                _logger.LogError("Problem with database while getting");
                result = StatusCode(500);
            }

            return result;
        }

        // GET /Nickname
        [HttpGet("{Nickname}")]
        public ActionResult<ReaderOutput> Get(string Nickname)
        {
            _logger.LogInformation($"Get book with name: {Nickname}");
            ActionResult<ReaderOutput> result = BadRequest();
            try
            {
                var list = database.Readers.Where(reader => reader.Nickname == Nickname);
                if (list.Any())
                {
                    var reader = list.First(); 
                    var nickname = reader.Nickname;
                    var books = database.ReadedBooks.Where(x => x.Reader.Nickname == nickname).Select(x => x.Name).ToList();
                    result = new ReaderOutput()
                    {
                        Nickname = nickname,
                        Books = books
                    };
                    _logger.LogInformation("Successful getting");
                }
                else
                    _logger.LogInformation("No reader with this nickname"); 
            }
            catch
            {
                _logger.LogError("Problem with database while getting");
                result = StatusCode(500);
            }

            return result; 
        }

        // POST /
        [HttpPost]
        public ActionResult Post([FromBody] string Nickname)
        {
            _logger.LogInformation($"Add reader: {Nickname}");
            ActionResult result = Ok();
            try
            {
                var reader = new Reader { Nickname = Nickname }; //, Books = new List<ReadedBook>() };
                if (!database.Readers.Where(x => x.Nickname == Nickname).Any())
                    database.Readers.Add(reader);
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

        // GET /Nickname/books
        [HttpGet("{Nickname}/books")]
        public ActionResult<IEnumerable<string>> GetReaderBooks(string Nickname)
        {
            _logger.LogInformation($"Get all books of reader: {Nickname}");
            ActionResult<IEnumerable<string>> result;
            try
            {
                var books  = database.ReadedBooks.Where(x => x.Reader.Nickname == Nickname).Select(x => x.Name);
                if (books.Any())
                {
                    result = books.ToList(); 
                    _logger.LogInformation("Succesful getting!"); 
                }
                else
                {
                    result = NoContent();
                    _logger.LogInformation("No books with this name"); 
                }
            }
            catch
            {
                _logger.LogError("Problem with database while getting");
                result = StatusCode(500);
            }

            return result;
        }

        // DELETE /book/Name
        [HttpDelete("book/{Name}")]
        public ActionResult DeleteBook(string Name)
        {
            _logger.LogInformation($"Delete from all lists book: {Name}");
            ActionResult result = Ok();
            try
            {
                database.ReadedBooks.RemoveRange(database.ReadedBooks.Where(x => x.Name == Name)); 
                database.SaveChanges();
                _logger.LogInformation("Successful deleting"); 
            }
            catch
            {
                _logger.LogError("Problem with database while deleting");
                result = StatusCode(500);
            }

            return result;
        }

        // POST /Nickname/book
        [HttpPost("{Nickname}/book")]
        public ActionResult AddReaderBook([FromBody] string Name, string Nickname)
        {
            _logger.LogInformation($"Add book {Name} to reader {Nickname}");
            ActionResult result = Ok();
            try
            {
                var readers = database.Readers.Where(x => x.Nickname == Nickname 
                    && !x.Books.Where(zz => zz.Name == Name).Any());                 
                if (readers.Any())
                {
                    var reader = readers.First();
                    database.ReadedBooks.Add(new ReadedBook { Name = Name, Reader = reader }); 
                    database.SaveChanges();
                    _logger.LogInformation("Succesful adding");
                }
                else
                {
                    result = BadRequest();
                    _logger.LogInformation("No reader with this nickname or book already adding"); 
                }
            }
            catch
            {
                _logger.LogError("Problem with database while adding");
                result = StatusCode(500);
            }
            return result;
        }

        // DELETE /Nickname/book/Name
        [HttpDelete("{Nickname}/book/{Name}")]
        public ActionResult DeleteReaderBook(string Nickname, string Name)
        {
            _logger.LogInformation($"Delete book {Name} from reader {Nickname}");
            ActionResult result = Ok();
            try
            {
                var readers = database.Readers.Where(x => x.Nickname == Nickname);
                if (readers.Any())
                {
                    database.ReadedBooks.RemoveRange(database.ReadedBooks.Where(x => 
                        x.Name == Name && x.Reader.Nickname == Nickname));
                    
                    database.SaveChanges();
                    _logger.LogInformation("Succesful deleting");
                }
                else
                {
                    result = NoContent();
                    _logger.LogInformation("No reader with this nickname");
                }
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
