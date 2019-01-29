using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http; 
using Gateway.Models.Authors; 

namespace Gateway.Services
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAuthors();
        Task<Author> GetAuthor(string Name);
        Task<HttpResponseMessage> AddAuthor(Author Author); 
    }
}
