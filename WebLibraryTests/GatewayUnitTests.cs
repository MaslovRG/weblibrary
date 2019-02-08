using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Gateway;
using Gateway.Controllers;
using Gateway.Models;
using Gateway.Models.Authors;
using Gateway.Models.Books;
using Gateway.Models.Readers;
using Gateway.Services;
using Gateway.Services.Implementation; 
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebLibraryTests
{
    [TestClass]
    public class GatewayUnitTests
    {
        ILogger<AuthorController> _loggerA = Mock.Of<ILogger<AuthorController>>();
        ILogger<BookController> _loggerB = Mock.Of<ILogger<BookController>>();
        ILogger<ReaderController> _loggerR = Mock.Of<ILogger<ReaderController>>();
        IAuthorService authorService = Mock.Of<IAuthorService>();
        IBookService bookService = Mock.Of<IBookService>();
        IReaderService readerService = Mock.Of<IReaderService>();
        private AuthorController ac;
        private BookController bc;
        private ReaderController rc;

        [TestInitialize]
        public void Initialize()
        {            
            authorService = Mock.Of<IAuthorService>();
            bookService = Mock.Of<IBookService>();
            readerService = Mock.Of<IReaderService>();
        }

        [TestMethod]
        public async Task Test1()
        {
            ac = new AuthorController(_loggerA, authorService);
            var result = await ac.Get(null, null);
        }

        [TestMethod]
        public async Task Test2()
        {
            ac = new AuthorController(_loggerA, authorService);
            var result = await ac.Get("Author");
        }

        [TestMethod]
        public async Task Test3()
        {
            ac = new AuthorController(_loggerA, authorService);
            var result = await ac.Post(new Author());
        }

        [TestMethod]
        public async Task Test4()
        {
            bc = new BookController(_loggerB, bookService, authorService, readerService);
            var result = await bc.Get(null, null);
        }

        [TestMethod]
        public async Task Test5()
        {
            bc = new BookController(_loggerB, bookService, authorService, readerService);
            var result = await bc.Get("Name");
        }

        [TestMethod]
        public async Task Test6()
        {
            bc = new BookController(_loggerB, bookService, authorService, readerService);
            var result = await bc.Post(null);
        }

        [TestMethod]
        public async Task Test7()
        {
            bc = new BookController(_loggerB, bookService, authorService, readerService);
            var result = await bc.Delete("Name");
        }

        [TestMethod]
        public async Task Test8()
        {
            bc = new BookController(_loggerB, bookService, authorService, readerService);
            var result = await bc.GetBooksByAuthor("Author", null, null);
        }

        [TestMethod]
        public async Task Test9()
        {
            rc = new ReaderController(_loggerR, readerService, bookService, authorService);
            var result = await rc.Get(null, null);
        }

        [TestMethod]
        public async Task Test10()
        {
            rc = new ReaderController(_loggerR, readerService, bookService, authorService);
            var result = await rc.Get("Name");
        }

        [TestMethod]
        public async Task Test11()
        {
            rc = new ReaderController(_loggerR, readerService, bookService, authorService);
            var result = await rc.Post("Name");
        }

        [TestMethod]
        public async Task Test12()
        {
            rc = new ReaderController(_loggerR, readerService, bookService, authorService);
            var result = await rc.Delete("Nickname", "Name");
        }

        [TestMethod]
        public async Task Test13()
        {
            rc = new ReaderController(_loggerR, readerService, bookService, authorService);
            var result = await rc.GetBooks("Nick", 1, 1);
        }

        [TestMethod]
        public async Task Test14()
        {
            rc = new ReaderController(_loggerR, readerService, bookService, authorService);
            var result = await rc.GetAuthors("Nick", 1, 2); 
        }
    }
}
