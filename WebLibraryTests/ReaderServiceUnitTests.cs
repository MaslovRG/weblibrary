using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ReaderService;
using ReaderService.Controllers;
using ReaderService.Models;
using Microsoft.Extensions.Logging;

namespace WebLibraryTests
{
    [TestClass]
    public class ReaderServiceUnitTests
    {
        private ILogger<ReaderController> _logger;
        private ReadersContext database;

        [TestInitialize]
        public void Initialize()
        {
            _logger = Mock.Of<ILogger<ReaderController>>();
            database = GetDatabase();
        }

        private ReadersContext GetDatabase(List<Reader> readers = null)
        {
            if (readers == null)
                readers = new List<Reader>();
            return Mock.Of<ReadersContext>(db =>
                db.Readers == GetReaders(readers)
                && db.ReadedBooks == GetReadedBooks(readers));
        }

        private bool ReaderEqual(Reader reader1, Reader reader2)
        {
            bool result = reader1.Nickname == reader2.Nickname; 
            if (result)
            {
                var readbook1 = reader1.Books;
                var readbook2 = reader2.Books;
                if (readbook1 != null && readbook2 != null)
                {
                    var rb1 = readbook1.ToList();
                    var rb2 = readbook2.ToList();
                    for (int i = 0; i < rb1.Count && result; i++)
                    {
                        result = rb1[i].Name == rb2[i].Name;
                    }
                }
                else
                    result = readbook1 == readbook2; 
            }
            return result; 
        }

        private bool OutputEqual(ReaderOutput ro1, ReaderOutput ro2)
        {
            bool result = ro1.Nickname == ro2.Nickname;
            if (result)
            {
                if (ro1.Books != null && ro2.Books != null)
                {
                    result = ro1.Books.Count == ro2.Books.Count; 
                    for (int i = 0; i < ro1.Books.Count && result; i++)
                    {
                        result = ro1.Books[i] == ro2.Books[i];
                    }
                }
                else
                    result = ro1.Books == ro2.Books;
            }
            return result;
        }

        private DbSet<Reader> GetReaders(List<Reader> readers)
        {
            var query = readers.AsQueryable();
            var mockSet = new Mock<DbSet<Reader>>();
            var mockQuery = mockSet.As<IQueryable<Reader>>();
            mockQuery.Setup(x => x.Provider).Returns(query.Provider);
            mockQuery.Setup(x => x.Expression).Returns(query.Expression);
            mockQuery.Setup(x => x.ElementType).Returns(query.ElementType);
            mockQuery.Setup(x => x.GetEnumerator()).Returns(query.GetEnumerator());

            mockSet.Setup(x => x.Add(It.IsAny<Reader>())).Callback<Reader>(x => readers.Add(x));
            return mockSet.Object;
        }

        private DbSet<ReadedBook> GetReadedBooks(List<Reader> readers)
        {
            var list = new List<ReadedBook>();
            foreach (var reader in readers)
            {
                var books = reader.Books.ToList();
                list.AddRange(books); 
            }
            var query = list.AsQueryable();  
            var mockSet = new Mock<DbSet<ReadedBook>>();
            var mockQuery = mockSet.As<IQueryable<ReadedBook>>();
            mockQuery.Setup(x => x.Provider).Returns(query.Provider);
            mockQuery.Setup(x => x.Expression).Returns(query.Expression);
            mockQuery.Setup(x => x.ElementType).Returns(query.ElementType);
            mockQuery.Setup(x => x.GetEnumerator()).Returns(query.GetEnumerator());

            mockSet.Setup(x => x.Add(It.IsAny<ReadedBook>()))
                .Callback<ReadedBook>(x =>
                {
                    if (x.Reader != null)
                    {
                        x.Reader.Books.Add(x);
                        list.Add(x); 
                    }
                });
            mockSet.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<ReadedBook>>()))
                .Callback<IEnumerable<ReadedBook>>(x =>
                {
                    var yyy = x.ToList(); 
                    foreach (var book in yyy)
                    {
                        list.Remove(book); 
                        foreach (var reader in readers)
                        {
                            reader.Books.Remove(book); 
                        }
                    }
                });

            return mockSet.Object; 
        }

        private Reader CreateReader(string nickname, List<string> books = null)
        {
            var reader = new Reader()
            {
                Nickname = nickname
            };
            if (books != null)
            {
                reader.Books = new List<ReadedBook>();
                foreach (var bookName in books)
                {
                    reader.Books.Add(new ReadedBook()
                    {
                        Name = bookName,
                        Reader = reader
                    });
                }
            }
            else
            {
                reader.Books = null; 
            }
            return reader; 
        }

        [TestMethod]
        public void Test1()
        {
            var service = new ReaderController(null, _logger);

            var result = service.Get();
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test2()
        {
            var service = new ReaderController(database, _logger);

            var result = service.Get();
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(204, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test3()
        {            
            var readers = new List<Reader>()
            {
                CreateReader("R1", new List<string>() { "B1", "B2", "B3" }),
                CreateReader("R2", new List<string>() { "B3", "B1", "B5" }),
                CreateReader("R3", new List<string>() { "B2", "B6", "B7" })
            };
            var readersOutput = new List<ReaderOutput>()
            {
                new ReaderOutput()
                {
                    Nickname = "R1", Books = new List<string>() { "B1", "B2", "B3" }
                },
                new ReaderOutput()
                {
                    Nickname = "R2", Books = new List<string>() { "B3", "B1", "B5" }
                },
                new ReaderOutput()
                {
                    Nickname = "R3", Books = new List<string>() { "B2", "B6", "B7" }
                }
            };
            database = GetDatabase(readers);
            var service = new ReaderController(database, _logger);

            var result = service.Get().Value.ToList(); 
            Assert.AreEqual(readersOutput.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(true, OutputEqual(readersOutput[i], result[i])); 
            }
        }

        [TestMethod]
        public void Test4()
        {
            var service = new ReaderController(null, _logger);

            var result = service.Get("R2");
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test5()
        {
            var service = new ReaderController(database, _logger);

            var result = service.Get("R2");
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(400, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test6()
        {
            var readers = new List<Reader>()
            {
                CreateReader("R1", new List<string>() { "B1", "B2", "B3" }),
                CreateReader("R2", new List<string>() { "B3", "B1", "B5" }),
                CreateReader("R3", new List<string>() { "B2", "B6", "B7" })
            };
            var readerOutput = new ReaderOutput()
            {
                Nickname = "R2",
                Books = new List<string>() { "B3", "B1", "B5" }
            }; 
            database = GetDatabase(readers);
            var service = new ReaderController(database, _logger);

            var result = service.Get("R2").Value;
            Assert.AreEqual(true, OutputEqual(readerOutput, result));
        }

        [TestMethod]
        public void Test7()
        {
            var service = new ReaderController(null, _logger);

            var result = service.Post("R4"); 
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test8()
        {
            var readers = new List<Reader>()
            {
                CreateReader("R1", new List<string>() { "B1", "B2", "B3" }),
                CreateReader("R2", new List<string>() { "B3", "B1", "B5" }),
                CreateReader("R3", new List<string>() { "B2", "B6", "B7" })
            };
            var readersOutput = new List<Reader>()
            {
                CreateReader("R1", new List<string>() { "B1", "B2", "B3" }),
                CreateReader("R2", new List<string>() { "B3", "B1", "B5" }),
                CreateReader("R3", new List<string>() { "B2", "B6", "B7" }),
                CreateReader("R4")
            };
            database = GetDatabase(readers);
            var service = new ReaderController(database, _logger);

            var rexp = service.Post("R4");
            var statusCode = (StatusCodeResult)rexp;
            Assert.AreEqual(200, statusCode.StatusCode);

            Assert.AreEqual(readers.Count, readersOutput.Count); 
            for (int i = 0; i < readers.Count; i++)
            {
                Assert.AreEqual(true, ReaderEqual(readersOutput[i], readers[i])); 
            }
        }

        [TestMethod]
        public void Test9()
        {
            var service = new ReaderController(null, _logger);

            var result = service.GetReaderBooks("R4");
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test10()
        {
            var service = new ReaderController(database, _logger);

            var result = service.GetReaderBooks("R4");
            Assert.AreEqual(null, result.Value);
            var statusCode = (StatusCodeResult)result.Result;
            Assert.AreEqual(204, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test11()
        {
            var list = new List<string>() { "B2", "B6", "B7" }; 
            var readers = new List<Reader>()
            {
                CreateReader("R1", new List<string>() { "B1", "B2", "B3" }),
                CreateReader("R2", new List<string>() { "B3", "B1", "B5" }),
                CreateReader("R3", list)
            };            
            database = GetDatabase(readers);
            var service = new ReaderController(database, _logger);

            var result = service.GetReaderBooks("R3").Value.ToList(); 

            Assert.AreEqual(list.Count, result.Count); 
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(list[i], result[i]); 
            }
        }

        [TestMethod]
        public void Test12()
        {
            var service = new ReaderController(null, _logger);

            var result = service.DeleteBook("B2");
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test13()
        {
            var readers = new List<Reader>()
            {
                CreateReader("R1", new List<string>() { "B1", "B2", "B3" }),
                CreateReader("R2", new List<string>() { "B3", "B1", "B5" }),
                CreateReader("R3", new List<string>() { "B2", "B6", "B7" })
            };
            var readersOutput = new List<Reader>()
            {
                CreateReader("R1", new List<string>() { "B1", "B3" }),
                CreateReader("R2", new List<string>() { "B3", "B1", "B5" }),
                CreateReader("R3", new List<string>() { "B6", "B7" }),
            };
            database = GetDatabase(readers);
            var service = new ReaderController(database, _logger);

            var rexp = service.DeleteBook("B2"); 
            var statusCode = (StatusCodeResult)rexp;
            Assert.AreEqual(200, statusCode.StatusCode);

            Assert.AreEqual(readers.Count, readersOutput.Count);
            for (int i = 0; i < readers.Count; i++)
            {
                Assert.AreEqual(true, ReaderEqual(readersOutput[i], readers[i]));
            }
        }

        [TestMethod]
        public void Test14()
        {
            var service = new ReaderController(null, _logger);

            var result = service.AddReaderBook("Name", "Nick"); 
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test15()
        {
            var service = new ReaderController(database, _logger);

            var result = service.AddReaderBook("Name", "Nick"); 
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(204, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test16()
        {
            var readers = new List<Reader>()
            {
                CreateReader("R1", new List<string>() { "B1", "B2", "B3" }),
                CreateReader("R2", new List<string>() { "B3", "B1", "B5" }),
                CreateReader("R3", new List<string>() { "B2", "B6", "B7" })
            };
            var readersOutput = new List<Reader>()
            {
                CreateReader("R1", new List<string>() { "B1", "B2", "B3" }),
                CreateReader("R2", new List<string>() { "B3", "B1", "B5" }),
                CreateReader("R3", new List<string>() { "B2", "B6", "B7", "B100" })
            };
            database = GetDatabase(readers);
            var service = new ReaderController(database, _logger);

            var rexp = service.AddReaderBook("B100", "R3");
            var statusCode = (StatusCodeResult)rexp;
            Assert.AreEqual(200, statusCode.StatusCode);

            Assert.AreEqual(readers.Count, readersOutput.Count);
            for (int i = 0; i < readers.Count; i++)
            {
                Assert.AreEqual(true, ReaderEqual(readersOutput[i], readers[i]));
            }
        }

        [TestMethod]
        public void Test17()
        {
            var service = new ReaderController(null, _logger);

            var result = service.DeleteReaderBook("Nick", "Name");
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(500, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test18()
        {
            var service = new ReaderController(database, _logger);

            var result = service.DeleteReaderBook("Nick", "Name");
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(204, statusCode.StatusCode);
        }

        [TestMethod]
        public void Test19()
        {
            var readers = new List<Reader>()
            {
                CreateReader("R1", new List<string>() { "B1", "B2", "B3" }),
                CreateReader("R2", new List<string>() { "B3", "B1", "B5" }),
                CreateReader("R3", new List<string>() { "B2", "B6", "B7" })
            };
            var readersOutput = new List<Reader>()
            {
                CreateReader("R1", new List<string>() { "B1", "B3" }),
                CreateReader("R2", new List<string>() { "B3", "B1", "B5" }),
                CreateReader("R3", new List<string>() { "B2", "B6", "B7" })
            };
            database = GetDatabase(readers);
            var service = new ReaderController(database, _logger);

            var rexp = service.DeleteReaderBook("R1", "B2");
            var statusCode = (StatusCodeResult)rexp;
            Assert.AreEqual(200, statusCode.StatusCode);

            Assert.AreEqual(readers.Count, readersOutput.Count);
            for (int i = 0; i < readers.Count; i++)
            {
                Assert.AreEqual(true, ReaderEqual(readersOutput[i], readers[i]));
            }
        }
    }
}
