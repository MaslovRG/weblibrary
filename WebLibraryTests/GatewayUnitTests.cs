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
        private IAuthorService authorService;
        private IBookService bookService;
        private IReaderService readerService; 
        private AuthorController ac;
        private BookController bc;
        private ReaderController rc; 

        [TestInitialize]
        public void Initialize()
        {
            ILogger<AuthorController> _loggerA = Mock.Of<ILogger<AuthorController>>();
            ILogger<BookController> _loggerB = Mock.Of<ILogger<BookController>>();
            ILogger<ReaderController> _loggerR = Mock.Of<ILogger<ReaderController>>();
            IAuthorService authorService = Mock.Of<IAuthorService>(); 
            IBookService bookService = Mock.Of<IBookService>();
            IReaderService readerService = Mock.Of<IReaderService>();
            
            
            ac = new AuthorController(_loggerA, authorService);
            bc = new BookController(_loggerB, bookService, authorService, readerService);
            rc = new ReaderController(_loggerR, readerService, bookService, authorService); 
        }

        [TestMethod]
        public async Task Test1()
        {
            await ac.Get(null, null); 
        }

        [TestMethod]
        public async Task Test2()
        {
            await ac.Get("Author"); 
        }

        [TestMethod]
        public async Task Test3()
        {
            await ac.Post(new Author()); 
        }
    }
}
