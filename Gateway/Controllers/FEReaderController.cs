using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gateway.Models;
using Gateway.Models.Readers;
using Gateway.Models.Books;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering; 

namespace Gateway.Controllers
{
    [Route("reader")]
    public class FEReaderController : Controller
    {
        //private AuthorController authorController;
        private BookController bookController;
        private ReaderController readerController;

        public FEReaderController(AuthorController nAC,
            BookController nBC, ReaderController nRC)
        {
            //authorController = nAC;
            bookController = nBC;
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

        [HttpGet("{Nickname}")]
        public async Task<IActionResult> Reader(string Nickname)
        {
            var result = await readerController.Get(Nickname);
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

        [HttpGet("addbook")]
        public async Task<IActionResult> AddBookToReader(string Name, string Nickname)
        {
            var booksRQ = await bookController.Get(null, null);
            var readersRQ = await readerController.Get(null, null); 
            if (booksRQ == null || readersRQ == null || booksRQ.StatusCode != 200 || readersRQ.StatusCode != 200)
            {
                return View("Error", new Error() { Code = 500, Message = "Can't get books or readers" }); 
            }
            PagedList<Book> books = (PagedList<Book>)booksRQ.Value;
            ViewBag.Books = books.Select(x => new SelectListItem(x.Name, x.Name)).ToList(); 
            PagedList<Reader> readers = (PagedList<Reader>)readersRQ.Value;
            ViewBag.Readers = readers.Select(x => new SelectListItem(x.Nickname, x.Nickname)).ToList(); 

            return View(new ReadedBook() { Name = Name, Nickname = Nickname }); 
        }

        [HttpPost("addbook")]
        public async Task<IActionResult> AddBookToReader(ReadedBook rb)
        {
            var result = await readerController.Post(rb.Nickname, rb.Name);
            if (result.StatusCode == 200)
                return RedirectToAction(nameof(Readers));
            return View("Error", new Error(result)); 
        }

        [HttpGet("{Nickname}/book/{Name}")]
        public IActionResult DeleteBookFromReader(string Nickname, string Name)
        {
            if (Nickname == null || Name == null)
            {
                return RedirectToAction(nameof(Readers)); 
            }

            return View(new ReadedBook() { Name = Name, Nickname = Nickname }); 
        }

        [HttpPost("{Nickname}/book/{Name}")]
        public async Task<IActionResult> DeleteBookFromReaderAction(string Nickname, string Name)
        {
            var result = await readerController.Delete(Nickname, Name);
            if (result.StatusCode == 200)
                return RedirectToAction(nameof(Readers));
            return View("Error", new Error(result));
        }

        [HttpGet("{Nickname}/books")]
        public async Task<IActionResult> ReaderBooks(string Nickname, int? page, int? size)
        {
            if (page == null || size == null)
            {
                var nPage = page ?? 1;
                var nSize = size ?? 2;
                return Redirect($"/reader/{Nickname}/books?page={nPage}&size={nSize}");
            }

            var result = await readerController.GetBooks(Nickname, page, size);
            if (result == null || result.StatusCode != 200)
            {
                Error error = new Error(result);
                return View("Error", error);
            }
            ViewBag.Nickname = Nickname; 

            return View(result.Value);
        }

        [HttpGet("{Nickname}/authors")]
        public async Task<IActionResult> ReaderAuthors(string Nickname, int? page, int? size)
        {
            if (page == null || size == null)
            {
                var nPage = page ?? 1;
                var nSize = size ?? 2;
                return Redirect($"/reader/{Nickname}/authors?page={nPage}&size={nSize}");
            }

            var result = await readerController.GetAuthors(Nickname, page, size);
            if (result == null || result.StatusCode != 200)
            {
                Error error = new Error(result);
                return View("Error", error);
            }
            ViewBag.Nickname = Nickname;

            return View(result.Value);
        }
    }
}