using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Gateway.Models.Books;
using System.Net;

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
            using (var handler = new HttpClientHandler() { CookieContainer = new CookieContainer() })
            using (var client = new HttpClient(handler))
            {
                try
                {
                    if (token != null && token.Value != null)
                        handler.CookieContainer.Add(new Cookie("appToken", token.Value)); 
                    return await client.PostAsJsonAsync(GetFullAddressByUrl(url), obj);
                }
                catch
                {
                    return null; 
                }
            }
        }

        protected async Task<HttpResponseMessage> Get(string url)
        {
            using (var handler = new HttpClientHandler() { CookieContainer = new CookieContainer() })
            using (var client = new HttpClient(handler) { BaseAddress = BA })
            {
                try
                {
                    if (token != null && token.Value != null)
                        handler.CookieContainer.Add(BA, new Cookie("appToken", token.Value));
                    return await client.GetAsync(GetFullAddressByUrl(url));
                }
                catch
                {
                    return null; 
                }
            }
        }

        protected async Task<HttpResponseMessage> Delete(string url)
        {
            using (var handler = new HttpClientHandler() { CookieContainer = new CookieContainer() })
            using (var client = new HttpClient(handler) { BaseAddress = BA })
            {
                try
                {
                    if (token != null && token.Value != null)
                        handler.CookieContainer.Add(BA, new Cookie("appToken", token.Value));
                    return await client.DeleteAsync(GetFullAddressByUrl(url)); 
                    /*var message = new HttpRequestMessage(HttpMethod.Delete, url);
                    message.Headers.Add("Coockies", $"appToken=")*/
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
            var response = await Get($"token/check/{token}");
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
            return new Result() { Code = 500, Message = "Can't authorized on reader service. Reload page" };
        }

        public Result<T> GetError<T>()
        {
            return new Result<T>() { Code = 500, Message = "Can't authorized on reader service. Reload page", Value = default(T) };
        }
    }
}
