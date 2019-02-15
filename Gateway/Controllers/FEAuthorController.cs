using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gateway.Models; 

namespace Gateway.Controllers
{
    [Route("author")]
    public class FEAuthorController : Controller
    {
        private AuthorController authorController;
        private BookController bookController;
        private ReaderController readerController; 

        public FEAuthorController(AuthorController nAC, 
            BookController nBC, ReaderController nRC)
        {
            authorController = nAC;
            bookController = nBC;
            readerController = nRC; 
        }

        [HttpGet("")]
        public async Task<IActionResult> Authors(int? page, int? size)
        {
            if (page == null || size == null)
            {
                var nPage = page ?? 1;
                var nSize = size ?? 2;
                return Redirect($"/author?page={nPage}&size={nSize}");
            }

            var result = await authorController.Get(page, size);

            if (result == null || result.StatusCode != 200)
            {
                Error error = new Error(result); 
                return View("Error", error); 
            }
            
            return View(result.Value);
        }
        
    }
}