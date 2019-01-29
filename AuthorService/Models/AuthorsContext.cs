using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 

namespace AuthorService.Models
{
    public class AuthorsContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public AuthorsContext(DbContextOptions<AuthorsContext> options)
            : base(options)
        { }
    }
}
