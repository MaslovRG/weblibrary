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
        {
            appInfo = new ServiceInfo()
            {
                AppId = configuration.GetSection("ReaderService")["appId"],
                AppSecret = configuration.GetSection("ReaderService")["appSecret"]
            };            
            token = null; 
        }

        public async Task<Result<List<Reader>>> GetReaders()
        {
            var result = await CheckToken();
            if (result == null || result.Code != 200)
                return GetError<List<Reader>>(); 

            var response = await Get("");
            return await Result<List<Reader>>.CreateAsync(response);
        }

        public async Task<Result<Reader>> GetReader(string Nickname)
        {
            var result = await CheckToken();
            if (result == null || result.Code != 200)
                return GetError<Reader>(); 

            var response = await Get(Nickname);
            return await Result<Reader>.CreateAsync(response);
        }

        public async Task<Result> AddReader(string Nickname)
        {
            var result = await CheckToken();
            if (result == null || result.Code != 200)
                return GetErrorNT(); 

            var response = await PostJson("", Nickname);
            return await Result.CreateAsync(response); 
        }

        public async Task<Result<List<string>>> GetReaderBooks(string Nickname)
        {
            var result = await CheckToken(); 
            if (result == null || result.Code != 200)
                return GetError<List<string>>(); 

            var response = await Get($"{Nickname}/books");
            return await Result<List<string>>.CreateAsync(response);
        }

        public async Task<Result> DeleteBook(string Name)
        {
            var result = await CheckToken();
            if (result == null || result.Code != 200)
                return GetErrorNT();

            var response = await Delete($"book/{Name}");
            return await Result.CreateAsync(response);
        }

        public async Task<Result> AddBookToReader(string Nickname, string Name)
        {
            var result = await CheckToken();
            if (result == null || result.Code != 200)
                return GetErrorNT(); 

            var response = await PostJson($"{Nickname}/book", Name);
            return await Result.CreateAsync(response);
        }

        public async Task<Result> DeleteBookFromReader(string Nickname, string Name)
        {
            var result = await CheckToken();
            if (result == null || result.Code != 200)
                return GetErrorNT();

            var response = await Delete($"{Nickname}/book/{Name}");
            return await Result.CreateAsync(response);
        }        
    }
}
