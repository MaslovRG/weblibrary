using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Gateway.Models.Books;
using System.Net;
using System.Text;

namespace Gateway.Services
{
    public class Service
    {
        private readonly string baseAddress;
        private readonly Uri BA; 
        protected ServiceInfo appInfo; 
        protected Token token; 

        public Service(string nBaseAddress)
        {
            baseAddress = nBaseAddress;
            BA = new Uri(nBaseAddress); 
        }

        private string GetFullAddressByUrl(string url)
        {
            return baseAddress + "/" + url; 
        }

        protected async Task<HttpResponseMessage> PostJson<T>(string url, T obj)
        {
            using (var handler = new HttpClientHandler() { UseCookies = false })
            using (var client = new HttpClient(handler))
            {
                try
                {
                    var message = new HttpRequestMessage(HttpMethod.Post, GetFullAddressByUrl(url));
                    message.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                    if (token != null && token.Value != null)
                        message.Headers.Add("Coockies", $"appToken={token.Value}");
                    return await client.SendAsync(message);
                }
                catch
                {
                    return null; 
                }
            }
        }

        protected async Task<HttpResponseMessage> Get(string url)
        {
            using (var handler = new HttpClientHandler() { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = BA })
            {
                try
                {
                    var message = new HttpRequestMessage(HttpMethod.Get, GetFullAddressByUrl(url)); 
                    if (token != null && token.Value != null)
                        message.Headers.Add("Coockies", $"appToken={token.Value};");
                    return await client.SendAsync(message);
                }
                catch
                {
                    return null; 
                }
            }
        }

        protected async Task<HttpResponseMessage> Delete(string url)
        {
            using (var handler = new HttpClientHandler() { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = BA })
            {
                try
                { 
                    var message = new HttpRequestMessage(HttpMethod.Delete, GetFullAddressByUrl(url));
                    if (token != null && token.Value != null)
                        message.Headers.Add("Coockie", $"appToken={token.Value}");
                    return await client.SendAsync(message); 
                            
                }
                catch
                {
                    return null; 
                }
            }
        }

        protected async Task<T> GetObjectOrNullFromJson<T>(HttpResponseMessage response)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            catch
            {
                return default(T);
            }
        }

        protected async Task<Result> CheckToken()
        {
            var response = await Get($"token/check/{token?.Value}");
            Result result = new Result() { Code = 500, Message = "Can't get token" };
            if (response == null || (int)response.StatusCode != 200)
            {
                response = await PostJson("token/get", appInfo);
                if (response != null && (int)response.StatusCode == 200)
                {
                    var tokenResult = await Result<Token>.CreateAsync(response);
                    if (tokenResult.Code == 200)
                    {
                        token = tokenResult.Value;
                        result = new Result() { Code = 200, Message = "Token succesfully get" };
                    }
                }
            }
            else
                result = new Result() { Code = 200, Message = "Token succesfully check" };             
            return result; 
        }

        public Result GetErrorNT()
        {
            return new Result() { Code = 500, Message = "Can't authorized on service. Reload page" };
        }

        public Result<T> GetError<T>()
        {
            return new Result<T>() { Code = 500, Message = "Can't authorized on service. Reload page", Value = default(T) };
        }
    }
}
