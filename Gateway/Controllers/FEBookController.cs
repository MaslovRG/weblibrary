using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gateway.Models;
using Gateway.Models.Books; 

namespace Gateway.Controllers
{
    [Route("book")]
    public class FEBookController : Controller
    {
        //private AuthorController authorController;
        private BookController bookController;
        //private ReaderController readerController;

        public FEBookController(AuthorController nAC,
            BookController nBC, ReaderController nRC)
        {
            //authorController = nAC;
            bookController = nBC;
            //readerController = nRC;
        }

        [HttpGet("")]
        public async Task<IActionResult> Books(int? page, int? size)
        {
            if (page == null || size == null)
            {
                var nPage = page ?? 1;
                var nSize = size ?? 2;
                return Redirect($"/book?page={nPage}&size={nSize}");
            }

            var result = await bookController.Get(page, size);
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
        public async Task<IActionResult> Book(string Name)
        {
            var result = await bookController.Get(Name);
            if (result == null || result.StatusCode != 200)
            {
                Error error = new Error(result);
                return View("Error", error);
            }

            return View(result.Value);
        }

        [HttpGet("add")]
        public IActionResult AddBook(string Name, string Author)
        {
            return View(new Book { Name = Name, Author = Author });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBook(Book book)
        {
            if (book != null && book.Author == "")
                book.Author = null;             
            var result = await bookController.Post(book);
            if (result.StatusCode == 200)
                return RedirectToAction(nameof(Books));
            return View("Error", new Error(result));
        }

        [HttpGet("author/{Name}")]
        public async Task<IActionResult> AuthorBooks(string Name, int? page, int? size)
        {
            if (page == null || size == null)
            {
                var nPage = page ?? 1;
                var nSize = size ?? 2;
                return Redirect($"/book/author/{Name}?page={nPage}&size={nSize}");
            }
            var result = await bookController.GetBooksByAuthor(Name, page, size);
            if (result == null || result.StatusCode != 200)
            {
                Error error = new Error(result);
                return View("Error", error);
            }
            return View(result.Value); 
        }

    }
}