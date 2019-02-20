using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using SessionService.Models; 

namespace SessionService.Controllers
{
    [Route("")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private UsersContext users;
        private TokensContext tokens;
        private readonly ILogger<SessionController> _logger;

        public SessionController(UsersContext nUsers, TokensContext nTokens, ILogger<SessionController> nLogger)
        {
            users = nUsers;
            tokens = nTokens; 
            _logger = nLogger;
        }

        [HttpPost("get")]
        public ObjectResult GetToken([FromBody]User user)
        {
            _logger.LogInformation("Get all authors");
            ObjectResult result = null;
            try
            {
                var pass = SHAConverter.GetHash(user.Password);
                var dbu = users.Users
                    .Where(x => x.Username == user.Username && x.Password == pass)
                    .FirstOrDefault(); 
                if (dbu == null)
                    result = StatusCode(401, "Wrong login or password"); 
                else
                {
                    Token token = new Token(dbu);
                    tokens.Tokens.RemoveRange(tokens.Tokens.Where(x => x.User == dbu.Username));
                    tokens.SaveChanges(); 
                    tokens.Tokens.Add(token);                    
                    tokens.SaveChanges();
                    TokenValue values = new TokenValue(token); 
                    return Ok(values); 
                }
            }
            catch
            {
                result = StatusCode(500, "Error with session service work"); 
            }                     
            return result;
        }
        
        [HttpPost("check")]
        public ObjectResult CheckToken([FromBody] string AccessToken)
        {
            _logger.LogInformation("Check token");
            ObjectResult result = null;
            try
            {
                var token = tokens.Tokens.FirstOrDefault(x => 
                    x.AccessToken == AccessToken);
                if (token == null)
                    result = StatusCode(401, "Can't find this tokens"); 
                else
                {
                    if (token.ExpirityAccess >= DateTime.Now)
                    {
                        token.ExpirityAccess = DateTime.Now.AddMinutes(30); 
                        result = Ok("Avaliable"); 
                    }
                    else
                        result = StatusCode(401, "AccessToken is die");
                }
            }
            catch
            {
                result = StatusCode(500, "Error with session service work");
            }
            return result; 
        }

        [HttpPost("refresh")]
        public ObjectResult RefreshToken([FromBody] string RefreshToken)
        {
            _logger.LogInformation("Refresh token");
            ObjectResult result = null;
            try
            {
                var token = tokens.Tokens.FirstOrDefault(x =>
                    x.RefreshToken == RefreshToken);
                if (token == null)
                    result = StatusCode(401, "Can't find this tokens");
                else
                {
                    var dbu = users.Users.FirstOrDefault(x =>
                        x.Username == token.User); 
                    if (dbu != null && token.ExpirityRefresh >= DateTime.Now)
                    {
                        Token after = new Token(dbu);
                        tokens.Tokens.Remove(token);
                        tokens.Tokens.Add(after);
                        tokens.SaveChanges(); 
                        result = Ok(new TokenValue(after));
                    }
                    else
                        result = StatusCode(401, "RefreshToken is die");
                }
            }
            catch
            {
                result = StatusCode(500, "Error with session service work");
            }
            return result; 
        }
    }
}
