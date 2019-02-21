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
        private TokensContext tokens; 
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(AuthorsContext nDatabase, TokensContext nTokens,
            ILogger<AuthorController> nLogger)
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
        // ObjectResult<IEnumerable<Author>>
        public ObjectResult Get()
        {
            var token = HttpContext.Request.Headers["Authorization"];
            if (token.Count < 1 || Check(token[0].Substring(7).TrimEnd(';')))
                return StatusCode(401, "Bad token");
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
            var token = HttpContext.Request.Headers["Authorization"];
            if (token.Count < 1 || Check(token[0].Substring(7).TrimEnd(';')))
                return StatusCode(401, "Bad token");
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
            var token = HttpContext.Request.Headers["Authorization"];
            if (token.Count < 1 || Check(token[0].Substring(7).TrimEnd(';')))
                return StatusCode(401, "Bad token");
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
                if (info.AppId == "author001" && info.AppSecret == "QheBzyLgq18ZdrVD")
                {
                    Token token = new Token()
                    {
                        Value = SHAConverter.GetHash(DateTime.Now.ToString()),
                        Expirity = DateTime.Now.AddHours(2)
                    };
                    tokens.Tokens.Add(token);
                    tokens.SaveChanges();
                    return Ok(token);
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

        private bool Check(string token)
        {
            var stoken = tokens.Tokens.FirstOrDefault(x => x.Value == token);           
            return stoken == null || stoken.Expirity <= DateTime.Now; 
        }
    }
}
