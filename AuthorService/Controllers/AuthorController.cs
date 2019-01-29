using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using AuthorService.Models; 

namespace AuthorService.Controllers
{
    [Route("")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private AuthorsContext database;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(AuthorsContext nDatabase, ILogger<AuthorController> nLogger)
        {
            database = nDatabase;
            _logger = nLogger;
        }

        // GET /
        [HttpGet]
        public ActionResult<IEnumerable<Author>> Get()
        {
            _logger.LogInformation("Get all authors");
            ActionResult<IEnumerable<Author>> result;
            try
            {
                if (database.Authors.Any())
                    result = database.Authors;
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

        // GET /Name
        [HttpGet("{Name}")]
        public ActionResult<Author> Get(string Name)
        {
            _logger.LogInformation($"Get author with name: {Name}");
            ActionResult<Author> result = BadRequest();
            try
            {
                var list = database.Authors.Where(book => book.Name == Name);
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

        // POST /
        [HttpPost]
        public ActionResult Post([FromBody]Author author)
        {
            _logger.LogInformation($"Add author: {author.Name}");
            ActionResult result = Ok();
            try
            {
                var Book = new Author { Name = author.Name };
                database.Authors.Add(Book);
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
    }
}
