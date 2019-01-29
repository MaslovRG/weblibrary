using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PagedList; 

namespace Gateway.Controllers
{
    public class SupportingFunctions
    {
        public static ActionResult GetResponseResult(HttpResponseMessage response)
        {
            if (response == null || !response.IsSuccessStatusCode)
                return new StatusCodeResult(500); 
            return new StatusCodeResult(200);
        }

        public static ActionResult<PagedList<T>> GetPagedList<T>(List<T> list, int? page, int? size)
        {
            if (list == null)
                return new StatusCodeResult(500); 
            ActionResult<PagedList<T>> result = new StatusCodeResult(204);
            if (list.Count != 0)
            {
                if (page != null && page > 0 && size != null && size > 0)
                    result = (PagedList<T>)list.ToPagedList((int)page, (int)size);
                else
                    result = (PagedList<T>)list.ToPagedList(1, list.Count);
            }
            return result;
        }
    }
}
