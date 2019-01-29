using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Gateway.Services;
using Gateway.Models.Readers;

namespace Gateway.Services.Implementation
{
    public class ReaderService : Service, IReaderService
    {
        public ReaderService(IConfiguration configuration) :
            base(configuration.GetSection("ServiceAddresses")["ReaderService"])
        { }

        public async Task<List<Reader>> GetReaders()
        {
            var response = await Get("");
            return JsonConvert.DeserializeObject<List<Reader>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Reader> GetReader(string Nickname)
        {
            var response = await Get(Nickname);
            return JsonConvert.DeserializeObject<Reader>(await response.Content.ReadAsStringAsync());
        }

        public async Task<HttpResponseMessage> AddReader(string Nickname)
        {
            return await PostJson("", Nickname);
        }

        public async Task<List<string>> GetReaderBooks(string Nickname)
        {
            var response = await Get("books/" + Nickname);
            return JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync()); 
        }

        public async Task<HttpResponseMessage> DeleteBook(string Name)
        {
            return await Delete("book/" + Name); 
        }

        public async Task<HttpResponseMessage> AddBookToReader(string Nickname, string Name)
        {
            return await PostJson($"book/{Nickname}", Name); 
        }

        public async Task<HttpResponseMessage> DeleteBookFromReader(string Nickname, string Name)
        {
            return await Delete($"book/{Nickname}/{Name}");
        }
    }
}
