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
        /*private ILogger<ReaderController> _logger;
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
                && db.ReadedBooks == GetReadedBooks());
        }

        private bool BookEqual(Reader reader1, Reader reader2)
        {

            return reader1.Nickname == reader2.Nickname; 
        }

        [TestMethod]
        public void Test1()
        {
            // empty
        }*/
    }
}
