using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Gateway.Models.Books; 

namespace Gateway.Services
{
    public class Service
    {
        private readonly string baseAddress;

        public Service(string nBaseAddress)
        {
            baseAddress = nBaseAddress;  
        }

        private string GetFullAddressByUrl(string url)
        {
            return baseAddress + "/" + url; 
        }

        protected async Task<HttpResponseMessage> PostJson<T>(string url, T obj)
        {
            using (var client = new HttpClient())
            {
                try
                {
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
            using (var client = new HttpClient())
            {
                try
                {
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
            using (var client = new HttpClient())
            {
                try
                {
                    return await client.DeleteAsync(GetFullAddressByUrl(url)); 
                }
                catch
                {
                    return null; 
                }
            }
        }
    }
}
