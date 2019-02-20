using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gateway.Models;
using Gateway.Models.Authors; 

namespace Gateway.Controllers
{
    [Route("author")]
    public class FEAuthorController : Controller
    {
        private AuthorController authorController;
        //private BookController bookController;
        //private ReaderController readerController; 
        private SessionController sessionController; 

        public FEAuthorController(AuthorController nAC, 
            BookController nBC, ReaderController nRC,
            SessionController nSC)
        {
            authorController = nAC;
            //bookController = nBC;
            //readerController = nRC; 
            sessionController = nSC; 
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

            var accessToken = Request.Cookies["AccessToken"]; 
            var response = await sessionController.CheckToken(accessToken);
            if (response == null || response.StatusCode != 200)
            {
                return RedirectToAction("Refresh", "FESession", new
                {
                    redirect = $"/author?page={page}&size={size}"
                });
            }            

            var result = await authorController.Get(page, size);
            if (result != null && result.StatusCode == 404)
                return View(); 
            if (result == null || result.StatusCode != 200)
            {
                Error error = new Error(result); 
                return View("Error", error); 
            }
            
            return View(result.Value);
        }

        [HttpGet("{Name}")]
        public async Task<IActionResult> Author(string Name)
        {
            var accessToken = Request.Cookies["AccessToken"];
            var response = await sessionController.CheckToken(accessToken);
            if (response == null || response.StatusCode != 200)
            {
                return RedirectToAction("Refresh", "FESession", new
                {
                    redirect = $"/author/{Name}"
                });
            }

            var result = await authorController.Get(Name); 
            if (result == null || result.StatusCode != 200)
            {
                Error error = new Error(result);
                return View("Error", error); 
            }

            return View(result.Value); 
        }

        [HttpGet("add")]
        public async Task<IActionResult> AddAuthor(string Name)
        {
            var accessToken = Request.Cookies["AccessToken"];
            var response = await sessionController.CheckToken(accessToken);
            if (response == null || response.StatusCode != 200)
            {
                return RedirectToAction("Refresh", "FESession", new
                {
                    redirect = $"/author/add?Name={Name}"
                });
            }
            return View(new Author { Name = Name }); 
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAuthor(Author author)
        {
            var accessToken = Request.Cookies["AccessToken"];
            var response = await sessionController.CheckToken(accessToken);
            if (response == null || response.StatusCode != 200)
            {
                return RedirectToAction("Refresh", "FESession", new
                {
                    redirect = $"/author/add"
                });
            }

            var result = await authorController.Post(author); 
            if (result.StatusCode == 200)
                return RedirectToAction(nameof(Authors));
            return View("Error", new Error(result));
        }
    }
}