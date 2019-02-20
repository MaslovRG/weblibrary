using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Gateway.Services
{
    public class Result
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public async static Task<Result> CreateAsync(HttpResponseMessage response)
        {
            Result result = new Result
            {
                Code = 500,
                Message = "No message found"
            };

            if (response == null)
            {
                result.Code = 503;
                result.Message = "Service unavaliable";
            }
            else
            {
                try
                {
                    var message = await response.Content.ReadAsStringAsync();
                    if (message != null && message.Length > 0)
                        result.Message = message;
                }
                catch
                {
                    result.Message = "Can't get message";
                }                
                var code = (int)response.StatusCode;
                if (code >= 100)
                    result.Code = code;                 
            }
            return result; 
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; set; }

        public async static new Task<Result<T>> CreateAsync(HttpResponseMessage response)
        {
            Result<T> result = new Result<T>
            {
                Code = 500,
                Value = default(T),
                Message = "No message found"
            }; 

            if (response == null)
            {
                result.Code = 503;
                result.Message = "Service unavaliable";
            }
            else
            {
                if ((int)response.StatusCode == 200)
                {
                    try
                    {
                        result.Value = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    }
                    catch
                    {
                        result.Value = default(T);
                    }
                }                
                if (result.Value == null || result.Value.Equals(default(T)) || (int)response.StatusCode != 200)
                {
                    try
                    {
                        var message = await response.Content.ReadAsStringAsync();
                        if (message != null && message.Length > 0)
                            result.Message = message;
                    }
                    catch
                    {
                        result.Message = "Can't get message";
                    }
                    var code = (int)response.StatusCode;
                    if (code >= 100)
                        result.Code = code;
                }
                else
                {
                    result.Code = 200;
                    result.Message = "Succesful getting object"; 
                }              
            }
            return result;
        }
    }


}
