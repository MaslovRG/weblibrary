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
        Task<List<Reader>> GetReaders();
        Task<Reader> GetReader(string Nickname);
        Task<HttpResponseMessage> AddReader(string Nickname);
        Task<List<string>> GetReaderBooks(string Nickname);
        Task<HttpResponseMessage> DeleteBook(string Name);
        Task<HttpResponseMessage> AddBookToReader(string Nickname, string Name);
        Task<HttpResponseMessage> DeleteBookFromReader(string Nickname, string Name); 
    }
}
