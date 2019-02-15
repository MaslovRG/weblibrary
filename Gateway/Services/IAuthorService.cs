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
        Task<Result<List<Author>>> GetAuthors();
        Task<Result<Author>> GetAuthor(string Name);
        Task<Result> AddAuthor(Author Author); 
    }
}
