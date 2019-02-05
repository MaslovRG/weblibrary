using System;
using System.Collections.Generic;
using System.Text;
using System.Linq; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Moq;
using AuthorService;
using AuthorService.Controllers;
using AuthorService.Models;
using Microsoft.Extensions.Logging; 

namespace WebLibraryTests
{
    [TestClass]
    public class AuthorServiceUnitTests
    {
        private ILogger<AuthorController> _logger;
        private AuthorsContext database; 

        [TestInitialize]
        public void Initialize()
        {
            _logger = Mock.Of<ILogger<AuthorController>>();
            database = GetDatabase(); 
        }

        private AuthorsContext GetDatabase(List<Author> authors = null)
        {
            if (authors == null)
                authors = new List<Author>();
            return Mock.Of<AuthorsContext>(db =>
                db.Authors == GetAuthors(authors));
        }

        private DbSet<Author> GetAuthors(List<Author> authors)
        {
            var query = authors.AsQueryable();
            var mockSet = new Mock<DbSet<Author>>();
            var mockQuery = mockSet.As<IQueryable<Author>>();
            mockQuery.Setup(x => x.Provider).Returns(query.Provider);
            mockQuery.Setup(x => x.Expression).Returns(query.Expression);
            mockQuery.Setup(x => x.ElementType).Returns(query.ElementType);
            mockQuery.Setup(x => x.GetEnumerator()).Returns(query.GetEnumerator());

            mockSet.Setup(x => x.Add(It.IsAny<Author>())).Callback<Author>(x => authors.Add(x)); 
            return mockSet.Object; 
        }

        [TestMethod]
        public void Test1()
        {
            var service = new AuthorController(null, _logger);

            var result = service.Get();
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(500, statusCode.StatusCode); 
        }
        
        [TestMethod]
        public void Test2()
        {
            var service = new AuthorController(database, _logger);

            var result = service.Get();
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(204, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test3()
        {
            List<Author> authors = new List<Author>()
            {
                new Author() { Name = "A1" },
                new Author() { Name = "B2" },
                new Author() { Name = "B3" }
            };
            database = GetDatabase(authors); 
            var service = new AuthorController(database, _logger);

            var result = service.Get();
            var list = result.Value.ToList();
            Assert.AreEqual(authors.Count, list.Count); 
            for (int i = 0; i < authors.Count; i++)
            {
                Assert.AreEqual(authors[i].Name, list[i].Name); 
            }
        }

        [TestMethod] 
        public void Test4()
        {
            var service = new AuthorController(null, _logger);

            var result = service.Get("B2");
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test5()
        {
            List<Author> authors = new List<Author>()
            {
                new Author() { Name = "A1" },
                new Author() { Name = "B2" },
                new Author() { Name = "B3" }
            };
            database = GetDatabase(authors);
            var service = new AuthorController(database, _logger);

            var result = service.Get("B400");
            var author = result.Value;
            var code = ((StatusCodeResult)result.Result).StatusCode;
            Assert.AreEqual(400, code);
            Assert.AreEqual(null, author); 
        }

        [TestMethod]
        public void Test6()
        {
            List<Author> authors = new List<Author>()
            {
                new Author() { Name = "A1" },
                new Author() { Name = "B2" },
                new Author() { Name = "B3" }
            };
            database = GetDatabase(authors);
            var service = new AuthorController(database, _logger);

            var result = service.Get("B2");
            var author = result.Value;
            Assert.AreEqual("B2", author.Name);
        }

        [TestMethod] 
        public void Test7()
        {
            var service = new AuthorController(null, _logger);
            Author author = new Author()
            {
                Name = "A1"
            };

            var result = service.Post(author);
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test8()
        {
            List<Author> authorsNew = new List<Author>()
            {
                new Author() { Name = "A1" },
                new Author() { Name = "B2" },
                new Author() { Name = "B3" }
            };
            List<Author> authors = new List<Author>(); 
            database = GetDatabase(authors);
            var service = new AuthorController(database, _logger);

            foreach (var author in authorsNew)
            {
                var code = service.Post(author);
                var statusCode = (StatusCodeResult)code;
                Assert.AreEqual(200, statusCode.StatusCode);
            }

            var result = service.Get();
            Assert.AreEqual(authorsNew.Count, authors.Count);
            for (int i = 0; i < authorsNew.Count; i++)
            {
                Assert.AreEqual(authorsNew[i].Name, authors[i].Name);
            }

        }
    }
}
