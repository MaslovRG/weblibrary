using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Gateway.Services;
using Gateway.Models.Readers;
using Gateway.Models.Books;
using Gateway.Models.Authors;
using Gateway.Models.Session; 
using X.PagedList;

namespace Gateway.Controllers
{
    [Route("api/session")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<ReaderController> _logger;
        private IReaderService readerService;
        private IBookService bookService;
        private IAuthorService authorService;
        private ISessionService sessionService;

        public SessionController(ILogger<ReaderController> nLogger,
            IReaderService nReaderService,
            IBookService nBookService,
            IAuthorService nAuthorService,
            ISessionService nSessionService)
        {
            _logger = nLogger;
            readerService = nReaderService;
            bookService = nBookService;
            authorService = nAuthorService;
            sessionService = nSessionService;
        }

        [HttpPost("get")]
        public async Task<ObjectResult> GetToken(User user)
        {
            _logger.LogInformation("Get token");
            var response = await sessionService.GetToken(user); 
            
            if (response == null)
            {
                _logger.LogInformation("Session service unavaliable");
                return StatusCode(503, "Session service unavaliable"); 
            }

            if (response.Code != 200)
            {
                _logger.LogInformation("Can't get token");
                return StatusCode(response.Code, response.Message);
            }

            _logger.LogInformation("Succesfully get token");
            return Ok(response.Value);
        }

        [HttpPost("check")]
        public async Task<ObjectResult> CheckToken(string AccessString)
        {
            _logger.LogInformation("Check token");
            var response = await sessionService.CheckToken(AccessString);

            if (response == null)
            {
                _logger.LogInformation("Session service unavaliable");
                return StatusCode(503, "Session service unavaliable");
            }

            if (response.Code != 200)
            {
                _logger.LogInformation("Can't check token");
                return StatusCode(response.Code, response.Message);
            }

            _logger.LogInformation("Succesfully check token");
            return Ok(response.Message);
        }

        [HttpPost("refresh")]
        public async Task<ObjectResult> Refresh(string RefreshToken)
        {
            _logger.LogInformation("Refresh token");
            var response = await sessionService.Refresh(RefreshToken);

            if (response == null)
            {
                _logger.LogInformation("Session service unavaliable");
                return StatusCode(503, "Session service unavaliable");
            }

            if (response.Code != 200)
            {
                _logger.LogInformation("Can't refresh token");
                return StatusCode(response.Code, response.Message);
            }

            _logger.LogInformation("Succesfully refresh token");
            return Ok(response.Value);
        }
    }
}