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
using System.Net.Http;

namespace Gateway.Controllers
{
    [Route("author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private IAuthorService authorService;
        private SupportingFunctions sup; 

        public AuthorController(ILogger<AuthorController> nLogger,
            IAuthorService nAuthorService)
        {
            sup = new SupportingFunctions(); 
            _logger = nLogger;
            authorService = nAuthorService; 
        }

        // GET: author?page=1&size=5
        [HttpGet]
        public async Task<ActionResult<PagedList<Author>>> Get(int? page, int? size)
        {
            _logger.LogInformation("Get authors"); 
            var response = await authorService.GetAuthors();

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error"); 
                return StatusCode(500, "Internal error"); 
            }

            if (response.Code != 200)
            {
                _logger.LogInformation("Can't get authors");
                return StatusCode(response.Code, response.Message); 
            }

            return GetPagedList(response.Value, page, size); 
        }

        // GET: author/Name
        [HttpGet("{Name}")]
        public async Task<ActionResult<Author>> Get(string Name)
        {
            _logger.LogInformation($"Get author: {Name}"); 
            var response = await authorService.GetAuthor(Name);

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error");
                return StatusCode(500, "Internal error");
            }

            if (response.Code != 200)
            {
                _logger.LogInformation("Can't get author");
                return StatusCode(response.Code, response.Message);
            }

            _logger.LogInformation("Succesfully get author"); 
            return Ok(response.Value); 
        }

        // POST: author
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Author author)
        {
            _logger.LogInformation("Add author"); 
            var response = await authorService.AddAuthor(author);

            if (response == null)
            {
                _logger.LogInformation("Internal gateway error");
                return StatusCode(500, "Internal error");
            }

            return StatusCode(response.Code, response.Message);  
        }

        public ActionResult<PagedList<T>> GetPagedList<T>(List<T> list, int? page, int? size)
        {
            if (list == null)
                return StatusCode(500, "Empty list");
            ActionResult<PagedList<T>> result = StatusCode(204); 
            if (list.Count != 0)
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
