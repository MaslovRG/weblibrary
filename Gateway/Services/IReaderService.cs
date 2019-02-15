using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http; 
using Gateway.Models.Readers; 

namespace Gateway.Services
{
    public interface IReaderService
    {
        Task<Result<List<Reader>>> GetReaders();
        Task<Result<Reader>> GetReader(string Nickname);
        Task<Result> AddReader(string Nickname);
        Task<Result<List<string>>> GetReaderBooks(string Nickname);
        Task<Result> DeleteBook(string Name);
        Task<Result> AddBookToReader(string Nickname, string Name);
        Task<Result> DeleteBookFromReader(string Nickname, string Name); 
    }
}
