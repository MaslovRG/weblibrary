using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis; 

namespace BookService.Models
{
    [ExcludeFromCodeCoverage]
    public class BooksContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        [ExcludeFromCodeCoverage]
        public BooksContext(DbContextOptions<BooksContext> options)
            : base(options)
        { }

        public BooksContext()
        { }
    }
}
