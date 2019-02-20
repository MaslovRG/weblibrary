using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Gateway.Services; 
using Gateway.Models.Session;

namespace Gateway.Services.Implementation
{
    public class SessionService : Service, ISessionService
    {
        public SessionService(IConfiguration configuration) :
            base(configuration.GetSection("ServiceAddresses")["SessionService"])
        { }

        public async Task<Result<string>> GetCode(User user)
        {
            var response = await PostJson("code", user);
            return await Result<string>.CreateAsync(response);
        }

        public async Task<Result<TokenValue>> GetToken(SimpleCode Code)
        {
            var response = await PostJson("token", Code);
            return await Result<TokenValue>.CreateAsync(response);
        }

        public async Task<Result> CheckToken(string AccessToken)
        {
            var response = await PostJson("check", AccessToken); 
            return await Result.CreateAsync(response);
        }

        public async Task<Result<TokenValue>> Refresh(string RefreshToken)
        {
            var response = await PostJson("refresh", RefreshToken);
            return await Result<TokenValue>.CreateAsync(response);
        }
    }
}
