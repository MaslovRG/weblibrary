using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using BookService;
using BookService.Controllers;
using BookService.Models;
using Microsoft.Extensions.Logging;

namespace WebLibraryTests
{
    [TestClass]
    public class BookServiceUnitTests
    {
        private ILogger<BookController> _logger;
        private BooksContext database;

        [TestInitialize]
        public void Initialize()
        {
            _logger = Mock.Of<ILogger<BookController>>();
            database = GetDatabase();
        }

        private BooksContext GetDatabase(List<Book> books = null)
        {
            if (books == null)
                books = new List<Book>();
            return Mock.Of<BooksContext>(db =>
                db.Books == GetAuthors(books));
        }

        private bool BookEqual(Book book1, Book book2)
        {
            return book1.Name == book2.Name
                && book1.Author == book2.Author
                && book1.Year == book2.Year;
        }

        private DbSet<Book> GetAuthors(List<Book> books)
        {
            var query = books.AsQueryable();
            var mockSet = new Mock<DbSet<Book>>();
            var mockQuery = mockSet.As<IQueryable<Book>>();
            mockQuery.Setup(x => x.Provider).Returns(query.Provider);
            mockQuery.Setup(x => x.Expression).Returns(query.Expression);
            mockQuery.Setup(x => x.ElementType).Returns(query.ElementType);
            mockQuery.Setup(x => x.GetEnumerator()).Returns(query.GetEnumerator());

            mockSet.Setup(x => x.Add(It.IsAny<Book>())).Callback<Book>(x => books.Add(x));
            mockSet.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<Book>>()))
                .Callback<IEnumerable<Book>>(x =>
                {
                    foreach (var book in x)
                    {
                        books.Remove(book);
                    }
                });
            return mockSet.Object;
        }

        [TestMethod]
        public void Test1()
        {
            var service = new BookController(null, _logger);

            var result = service.Get();
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test2()
        {
            var service = new BookController(database, _logger);

            var result = service.Get();
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(204, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test3()
        {
            List<Book> books = new List<Book>()
            {
                new Book() { Name = "A1", Author = "Aut1", Year = 1929 },
                new Book() { Name = "B2", Author = "Aut2", Year = 1928 },
                new Book() { Name = "B3", Author = "Aut1", Year = 2012 }
            };
            database = GetDatabase(books);
            var service = new BookController(database, _logger);

            var result = service.Get();
            var list = result.Value.ToList();
            Assert.AreEqual(books.Count, list.Count);
            for (int i = 0; i < books.Count; i++)
            {
                Assert.AreEqual(true, BookEqual(books[i], list[i]));
            }
        }

        [TestMethod]
        public void Test4()
        {
            var service = new BookController(null, _logger);

            var result = service.Get("B2");
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test5()
        {
            List<Book> books = new List<Book>()
            {
                new Book() { Name = "A1", Author = "Aut1", Year = 1929 },
                new Book() { Name = "B2", Author = "Aut2", Year = 1928 },
                new Book() { Name = "B3", Author = "Aut1", Year = 2012 }
            };
            database = GetDatabase(books);
            var service = new BookController(database, _logger);

            var result = service.Get("B400");
            var book = result.Value;
            var code = ((StatusCodeResult)result.Result).StatusCode;
            Assert.AreEqual(400, code);
            Assert.AreEqual(null, book);
        }

        [TestMethod]
        public void Test6()
        {
            var gettedBook = new Book() { Name = "B2", Author = "Aut2", Year = 1928 };
            List<Book> books = new List<Book>()
            {
                new Book() { Name = "A1", Author = "Aut1", Year = 1929 },
                gettedBook,
                new Book() { Name = "B3", Author = "Aut1", Year = 2012 }
            };
            database = GetDatabase(books);
            var service = new BookController(database, _logger);

            var result = service.Get("B2");
            var book = result.Value;
            Assert.AreEqual(true, BookEqual(gettedBook, book));
        }

        [TestMethod]
        public void Test7()
        {
            var service = new BookController(null, _logger);
            Book book = new Book()
            {
                Name = "A101",
                Author = "Aut345",
                Year = 20
            };

            var result = service.Post(book);
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test8()
        {
            List<Book> booksNew = new List<Book>()
            {
                new Book() { Name = "A1", Author = "Au1", Year = 2000 },
                new Book() { Name = "B2", Author = "Au2", Year = 1444 },
                new Book() { Name = "B3", Author = "Au3", Year = 2000 }
            };
            List<Book> books = new List<Book>();
            database = GetDatabase(books);
            var service = new BookController(database, _logger);

            foreach (var book in booksNew)
            {
                var code = service.Post(book);
                var statusCode = (StatusCodeResult)code;
                Assert.AreEqual(200, statusCode.StatusCode);
            }

            var result = service.Get();
            Assert.AreEqual(booksNew.Count, books.Count);
            for (int i = 0; i < booksNew.Count; i++)
            {
                Assert.AreEqual(true, BookEqual(booksNew[i], books[i]));
            }
        }

        [TestMethod]
        public void Test9()
        {
            var service = new BookController(null, _logger);

            var result = service.Delete("A1");
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test10()
        {
            List<Book> books = new List<Book>()
            {
                new Book() { Name = "A1", Author = "Au1", Year = 2000 },
                new Book() { Name = "B2", Author = "Au2", Year = 1444 },
                new Book() { Name = "B3", Author = "Au3", Year = 2000 }
            };
            List<Book> booksNew = new List<Book>()
            {
                new Book() { Name = "A1", Author = "Au1", Year = 2000 },
                new Book() { Name = "B2", Author = "Au2", Year = 1444 },
                new Book() { Name = "B3", Author = "Au3", Year = 2000 }
            };
            database = GetDatabase(books);
            var service = new BookController(database, _logger);

            var result = service.Delete("Not a book");

            Assert.AreEqual(booksNew.Count, books.Count);
            for (int i = 0; i < booksNew.Count; i++)
            {
                Assert.AreEqual(true, BookEqual(booksNew[i], books[i]));
            }
        }

        [TestMethod]
        public void Test11()
        {
            List<Book> books = new List<Book>()
            {
                new Book() { Name = "A1", Author = "Au1", Year = 2000 },
                new Book() { Name = "B2", Author = "Au2", Year = 1444 },
                new Book() { Name = "B3", Author = "Au3", Year = 2000 }
            };
            List<Book> booksNew = new List<Book>()
            {
                new Book() { Name = "A1", Author = "Au1", Year = 2000 },
                new Book() { Name = "B3", Author = "Au3", Year = 2000 }
            };
            database = GetDatabase(books);
            var service = new BookController(database, _logger);

            var result = service.Delete("B2");

            Assert.AreEqual(booksNew.Count, books.Count);
            for (int i = 0; i < booksNew.Count; i++)
            {
                Assert.AreEqual(true, BookEqual(booksNew[i], books[i]));
            }
        }

        [TestMethod]
        public void Test12()
        {
            var service = new BookController(null, _logger);

            var result = service.GetBooksByAuthor("SimpleAuthor"); 
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test13()
        {
            List<Book> books = new List<Book>()
            {
                new Book() { Name = "A1", Author = "Au1", Year = 2000 },
                new Book() { Name = "B2", Author = "Au2", Year = 1444 },
                new Book() { Name = "B3", Author = "Au3", Year = 2000 }
            };            
            database = GetDatabase(books);
            var service = new BookController(database, _logger);

            var result = service.GetBooksByAuthor("Simple Author");

            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(400, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test14()
        {
            List<Book> books = new List<Book>()
            {
                new Book() { Name = "A1", Author = "Au1", Year = 2000 },
                new Book() { Name = "A2", Author = "Au1", Year = 200  },
                new Book() { Name = "A3", Author = "Au1", Year = 2019 },
                new Book() { Name = "A4", Author = "Au1", Year = 1997 },
                new Book() { Name = "B2", Author = "Au2", Year = 1486 },
                new Book() { Name = "B3", Author = "Au2", Year = 1555 },
                new Book() { Name = "B4", Author = "Au2", Year = 1444 },
                new Book() { Name = "CC", Author = "Au3", Year = 2000 }
            };
            List<Book> booksFound = new List<Book>()
            {
                new Book() { Name = "B2", Author = "Au2", Year = 1486 },
                new Book() { Name = "B3", Author = "Au2", Year = 1555 },
                new Book() { Name = "B4", Author = "Au2", Year = 1444 }
            };
            database = GetDatabase(books);
            var service = new BookController(database, _logger);

            var result = service.GetBooksByAuthor("Au2");

            var booksNew = (List<Book>)result.Value;
            Assert.AreEqual(booksFound.Count, booksNew.Count);
            for (int i = 0; i < booksFound.Count; i++)
            {
                Assert.AreEqual(true, BookEqual(booksFound[i], booksNew[i]));
            }
        }
    }
}
