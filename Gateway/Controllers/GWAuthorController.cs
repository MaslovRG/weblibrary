using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Gateway.Services;
using Gateway.Models.Authors; 
using PagedList; 

namespace Gateway.Controllers
{
    [Route("author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private IAuthorService authorService; 

        public AuthorController(ILogger<AuthorController> nLogger,
            IAuthorService nAuthorService)
        {
            _logger = nLogger;
            authorService = nAuthorService; 
        }

        // GET: author?page=1&size=5
        [HttpGet]
        public async Task<ActionResult<PagedList<Author>>> Get(int? page, int? size)
        {
            _logger.LogInformation("Get authors"); 
            var authors = await authorService.GetAuthors();
            return SupportingFunctions.GetPagedList(authors, page, size); 
        }

        // GET: author/Name
        [HttpGet("{Name}")]
        public async Task<ActionResult<Author>> Get(string Name)
        {
            _logger.LogInformation($"Get author: {Name}"); 
            var author = await authorService.GetAuthor(Name);

            if (author == null)
            {
                _logger.LogInformation("Can't find author"); 
                return NotFound();
            }

            return author; 
        }

        // POST: author
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Author author)
        {
            _logger.LogInformation("Add author"); 
            var response = await authorService.AddAuthor(author);
            return SupportingFunctions.GetResponseResult(response); 
        }        
    }
}
