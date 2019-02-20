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
        private CodesContext codes; 
        private TokensContext tokens;
        private readonly ILogger<SessionController> _logger;

        public SessionController(UsersContext nUsers, CodesContext nCodes,
            TokensContext nTokens, ILogger<SessionController> nLogger)
        {
            users = nUsers;
            codes = nCodes; 
            tokens = nTokens; 
            _logger = nLogger;           
        }

        [HttpPost("code")]
        public ObjectResult GetSecretCode([FromBody]User user)
        {
            _logger.LogInformation("Get token");
            ObjectResult result = null;
            try
            {
                var pass = SHAConverter.GetHash(user.Password);
                var dbu = users.Users
                    .FirstOrDefault(x => x.Username == user.Username && x.Password == pass);
                if (dbu == null)
                    result = StatusCode(401, "Wrong login or password");
                else
                {
                    Code code = new Code(dbu);
                    codes.Codes.RemoveRange(codes.Codes.Where(x => x.Username == dbu.Username));
                    codes.SaveChanges(); 
                    codes.Add(code);
                    codes.SaveChanges(); 
                    result = Ok(code.CodeValue);
                }
            }
            catch
            {
                result = StatusCode(500, "Error with session service work");
            }
            return result;
        }

        [HttpPost("token")]
        public ObjectResult GetToken([FromBody]SimpleCode SimpleCode)
        {
            var CodeValue = SimpleCode.CodeValue; 
            _logger.LogInformation("Get token");
            ObjectResult result = null;
            try
            {
                var Code = codes.Codes.FirstOrDefault(x => x.CodeValue == CodeValue);              

                if (Code == null)
                    result = StatusCode(401, "Wrong code value"); 
                else
                {
                    var dbu = users.Users.FirstOrDefault(x => x.Username == Code.Username);
                    if (dbu == null)
                        result = StatusCode(401, "User, connected with this code, not found");
                    else
                    {
                        Token token = new Token(dbu);
                        tokens.Tokens.RemoveRange(tokens.Tokens.Where(x => x.User == Code.Username));
                        tokens.SaveChanges();
                        tokens.Tokens.Add(token);
                        tokens.SaveChanges();
                        codes.Codes.RemoveRange(codes.Codes.Where(x => x.Username == Code.Username));
                        codes.SaveChanges(); 
                        TokenValue values = new TokenValue(token);
                        result = Ok(values);
                    }                    
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
