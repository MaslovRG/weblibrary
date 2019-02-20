using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http; 
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json; 
using Gateway.Services;
using Gateway.Models.Authors;

namespace Gateway.Services.Implementation
{
    public class AuthorService : Service, IAuthorService
    {
        public AuthorService(IConfiguration configuration) :
            base(configuration.GetSection("ServiceAddresses")["AuthorService"])
        {
            appInfo = new ServiceInfo()
            {
                AppId = configuration.GetSection("AuthorService")["appId"],
                AppSecret = configuration.GetSection("AuthorService")["appSecret"]
            };
            token = null;
        }

        public async Task<Result<List<Author>>> GetAuthors()
        {
            var result = await CheckToken();
            if (result == null || result.Code != 200)
                return GetError<List<Author>>();

            var response = await Get("");
            return await Result<List<Author>>.CreateAsync(response);  
        }

        public async Task<Result<Author>> GetAuthor(string Name)
        {
            var result = await CheckToken();
            if (result == null || result.Code != 200)
                return GetError<Author>();

            var response = await Get(Name);
            return await Result<Author>.CreateAsync(response);
        }

        public async Task<Result> AddAuthor(Author Author)
        {
            var result = await CheckToken();
            if (result == null || result.Code != 200)
                return GetErrorNT();

            var response = await PostJson("", Author);
            return await Result.CreateAsync(response); 
        }
    }
}
