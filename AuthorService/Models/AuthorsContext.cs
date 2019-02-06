using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AuthorService.Models
{
    [ExcludeFromCodeCoverage]
    public class AuthorsContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        [ExcludeFromCodeCoverage]
        public AuthorsContext(DbContextOptions<AuthorsContext> options)
            : base(options)
        { }
        
        public AuthorsContext()
        { }
    }
}
