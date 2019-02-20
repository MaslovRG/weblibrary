﻿using System;
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
        protected ServiceInfo appInfo; 
        protected string token; 

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
                    var tokenResult = await Result<string>.CreateAsync(response);
                    if (tokenResult.Code == 200)
                    {
                        token = tokenResult.Message;
                        result = new Result() { Code = 200, Message = "Token succesfully get" };
                    }
                }
            }
            else
                result = new Result() { Code = 200, Message = "Token succesfully check" };             
            return result; 
        }
    }
}
