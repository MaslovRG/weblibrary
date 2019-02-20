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
        private TokensContext tokens; 
        private readonly ILogger<ReaderController> _logger; 

        public ReaderController(ReadersContext nDatabase, TokensContext nTokens,
            ILogger<ReaderController> nLogger)
        {
            database = nDatabase;
            tokens = nTokens; 
            _logger = nLogger; 
        }

        // GET /
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        // ObjectResult<IEnumerable<ReaderOutput>>
        public ObjectResult Get()
        {            
            _logger.LogInformation("Get all readers");
            string message; 
            ObjectResult result;
            try
            {
                var readers = new List<ReaderOutput>(); 
                if (database.Readers.Any())
                {
                    foreach (var reader in database.Readers)
                    {
                        var books = database.ReadedBooks
                            .Where(x => x.Reader.Nickname == reader.Nickname)
                            .Select(x => x.Name)
                            .ToList(); 
                        var nickname = reader.Nickname;
                        var addedreader = new ReaderOutput()
                        {
                            Nickname = nickname,
                            Books = books
                        };
                        readers.Add(addedreader); 
                    }
                    message = "Successful getting"; 
                    result = Ok(readers);
                    _logger.LogInformation(message);
                }
                else
                {
                    message = "No readers found"; 
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

        // GET /Nickname
        [HttpGet("{Nickname}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        // ObjectResult<ReaderOutput>
        public ObjectResult Get(string Nickname)
        {
            _logger.LogInformation($"Get book with name: {Nickname}");
            string message; 
            ObjectResult result;
            try
            {
                var list = database.Readers
                    .Where(reader => reader.Nickname == Nickname);
                if (list.Any())
                {
                    var reader = list.First();
                    var nickname = reader.Nickname;
                    var books = database.ReadedBooks
                        .Where(x => x.Reader.Nickname == nickname)
                        .Select(x => x.Name)
                        .ToList();

                    message = "Successful getting";
                    result = Ok(new ReaderOutput()
                    {
                        Nickname = nickname,
                        Books = books
                    });
                    _logger.LogInformation(message);                    
                }
                else
                {
                    message = "No reader with this nickname found";
                    result = NotFound(message);
                    _logger.LogInformation(message);                     
                }
            }
            catch
            {
                message = "Problem with database while getting";
                _logger.LogError(message);
                result = StatusCode(500, message);
            }
            return result; 
        }

        // POST /
        [HttpPost]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public ObjectResult Post([FromBody] string Nickname)
        {
            _logger.LogInformation($"Add reader: {Nickname}");
            string message; 
            ObjectResult result;
            try
            {
                var reader = new Reader { Nickname = Nickname }; 
                    //, Books = new List<ReadedBook>() };

                if (!database.Readers.Where(x => x.Nickname == Nickname).Any())
                    database.Readers.Add(reader);
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

        // GET /Nickname/books
        [HttpGet("{Nickname}/books")]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        // ObjectResult<IEnumerable<string>>
        public ObjectResult GetReaderBooks(string Nickname)
        {
            _logger.LogInformation($"Get all books of reader: {Nickname}");
            string message; 
            ObjectResult result;
            try
            {
                var books  = database.ReadedBooks
                    .Where(x => x.Reader.Nickname == Nickname)
                    .Select(x => x.Name)
                    .ToList();

                if (books.Any())
                {
                    message = "Succesful getting!"; 
                    result = Ok(books); 
                    _logger.LogInformation(message); 
                }
                else
                {
                    message = "No books in user list found";
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

        // DELETE /book/Name
        [HttpDelete("book/{Name}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public ObjectResult DeleteBook(string Name)
        {
            _logger.LogInformation($"Delete from all lists book: {Name}");
            string message; 
            ObjectResult result;
            try
            {
                database.ReadedBooks
                    .RemoveRange(database.ReadedBooks.Where(x => x.Name == Name)); 
                database.SaveChanges();
                message = "Succesful deleting"; 
                result = Ok(message); 
                _logger.LogInformation(message); 
            }
            catch
            {
                result = StatusCode(500, "Problem with database while deleting");
                _logger.LogError("Problem with database while deleting");                
            }
            return result;
        }

        // POST /Nickname/book
        [HttpPost("{Nickname}/book")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public ObjectResult AddReaderBook([FromBody] string Name, string Nickname)
        {
            _logger.LogInformation($"Add book {Name} to reader {Nickname}");
            string message; 
            ObjectResult result;
            try
            {
                var readers = database.Readers.Where(x => x.Nickname == Nickname 
                    && !x.Books.Where(zz => zz.Name == Name).Any());                 
                if (readers.Any())
                {
                    var reader = readers.First();
                    database.ReadedBooks.Add(new ReadedBook
                        { Name = Name, Reader = reader }); 
                    database.SaveChanges();
                    message = "Succesful adding"; 
                    result = Ok(message); 
                    _logger.LogInformation(message);
                }
                else
                {
                    message = "No reader with this nickname found or book already adding";
                    result = BadRequest(message);
                    _logger.LogInformation(message); 
                }
            }
            catch
            {
                message = "Problem with database while adding"; 
                result = StatusCode(500, message);
                _logger.LogError(message);                
            }
            return result;
        }

        // DELETE /Nickname/book/Name
        [HttpDelete("{Nickname}/book/{Name}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public ObjectResult DeleteReaderBook(string Nickname, string Name)
        {
            _logger.LogInformation($"Delete book {Name} from reader {Nickname}");
            string message; 
            ObjectResult result;
            try
            {
                var readers = database.Readers.Where(x => x.Nickname == Nickname);
                if (readers.Any())
                {
                    database.ReadedBooks.RemoveRange(database.ReadedBooks.Where(x => 
                        x.Name == Name && x.Reader.Nickname == Nickname));
                    
                    database.SaveChanges();
                    message = "Succesful deleting"; 
                    result = Ok(message); 
                    _logger.LogInformation(message);
                }
                else
                {
                    message = "No reader with this nickname found"; 
                    result = BadRequest(message);
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

        [HttpGet("token/check/{token}")]
        public ObjectResult CheckToken(string token)
        {
            _logger.LogInformation("Check token"); 
            try
            {
                var stoken = tokens.Tokens.FirstOrDefault(x => x.Value == token);
                if (stoken == null)
                    return StatusCode(401, "Token not found");
                if (stoken.Expirity <= DateTime.Now)
                {
                    tokens.Tokens.Remove(stoken);
                    tokens.SaveChanges();
                    return StatusCode(401, "Token is die");
                }
            }
            catch
            {
                return StatusCode(500, "Error while checking token"); 
            }           

            return Ok("Token checked"); 
        }

        [HttpPost("token/get")]
        public ObjectResult GetToken(ServiceInfo info)
        {
            _logger.LogInformation("Get new token");
            try
            {
                if (info.AppId == "reader003" && info.AppSecret == "xsAlBhMmuoXGibuL")
                {
                    Token token = new Token()
                    {
                        Value = SHAConverter.GetHash(DateTime.Now.ToString()),
                        Expirity = DateTime.Now.AddHours(2)
                    };
                    tokens.Tokens.Add(token);
                    tokens.SaveChanges();
                    return Ok(token.Value);
                }
                else
                {
                    return StatusCode(401, "Your app dates is false"); 
                }
            }
            catch
            {
                return StatusCode(500, "Error while getting token"); 
            } 
        }
    }
}
