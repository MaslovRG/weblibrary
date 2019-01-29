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
            base(configuration.GetSection("ServiceAddresses")["AuthorService"]) { }

        public async Task<List<Author>> GetAuthors()
        {
            var response = await Get("");
            return JsonConvert.DeserializeObject<List<Author>>(await response.Content.ReadAsStringAsync()); 
        }

        public async Task<Author> GetAuthor(string Name)
        {
            var response = await Get(Name);
            return JsonConvert.DeserializeObject<Author>(await response.Content.ReadAsStringAsync()); 
        }

        public async Task<HttpResponseMessage> AddAuthor(Author Author)
        {
            return await PostJson("", Author); 
        }
    }
}
