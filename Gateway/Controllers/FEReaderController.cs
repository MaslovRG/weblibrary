using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gateway.Models;
using Gateway.Models.Readers; 

namespace Gateway.Controllers
{
    [Route("reader")]
    public class FEReaderController : Controller
    {
        //private AuthorController authorController;
        //private BookController bookController;
        private ReaderController readerController;

        public FEReaderController(AuthorController nAC,
            BookController nBC, ReaderController nRC)
        {
            //authorController = nAC;
            //bookController = nBC;
            readerController = nRC;
        }

        [HttpGet("")]
        public async Task<IActionResult> Readers(int? page, int? size)
        {
            if (page == null || size == null)
            {
                var nPage = page ?? 1;
                var nSize = size ?? 2;
                return Redirect($"/reader?page={nPage}&size={nSize}");
            }

            var result = await readerController.Get(page, size);
            if (result != null && result.StatusCode == 404)
                return View();
            if (result == null || result.StatusCode != 200)
            {
                Error error = new Error(result);
                return View("Error", error);
            }

            return View(result.Value);
        }

        [HttpGet("add")]
        public IActionResult AddReader(string Nickname)
        {
            return View(new Reader { Nickname = Nickname });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddReader(Reader reader)
        {
            string Nickname = reader.Nickname; 
            var result = await readerController.Post(Nickname);
            if (result.StatusCode == 200)
                return RedirectToAction(nameof(Readers));
            return View("Error", new Error(result));
        }
    }
}