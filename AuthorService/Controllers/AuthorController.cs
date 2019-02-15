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
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        // ObjectResult<IEnumerable<Author>>
        public ObjectResult Get()
        {
            _logger.LogInformation("Get all authors");
            string message; 
            ObjectResult result;
            try
            {
                if (database.Authors.Any())
                {
                    message = "Succesful getting";
                    result = Ok(database.Authors);                    
                    _logger.LogInformation(message); 
                }
                else
                {
                    message = "No authors found";
                    result = StatusCode(404, message);
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
        // ObjectResult<Author>
        public ObjectResult Get(string Name)
        {
            _logger.LogInformation($"Get author with name: {Name}");
            string message; 
            ObjectResult result;
            try
            {
                var list = database.Authors.Where(author => author.Name == Name);
                if (list.Any())
                {
                    message = "Succesful getting";
                    result = Ok(list.First());                    
                    _logger.LogInformation(message);
                }
                else
                {
                    message = "No authors with this name found";
                    result = StatusCode(404, message); 
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
        public ObjectResult Post([FromBody]Author author)
        {
            _logger.LogInformation($"Add author: {author.Name}");
            string message; 
            ObjectResult result;
            try
            {
                var Book = new Author { Name = author.Name };
                if (!database.Authors.Where(x => x.Name == author.Name).Any())
                    database.Authors.Add(Book);
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
    }
}
