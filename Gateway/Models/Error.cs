using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc; 

namespace Gateway.Models
{
    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public Error()
        {

        }

        public Error(ObjectResult objectResult)
        {
            Message = "Internal error";

            if (objectResult == null || objectResult.StatusCode == null)
                Code = 500;
            else
            {
                Code = (int)objectResult.StatusCode;
                if (objectResult.Value != null)
                    Message = objectResult.Value as string; 
            }
        }
    }
}
