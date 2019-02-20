using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ReaderService.Models
{
    public class TokensContext : DbContext
    {
        public DbSet<Token> Tokens { get; set; }
        public TokensContext(DbContextOptions<TokensContext> options)
            : base(options)
        { }

        public TokensContext()
        { }
    }
}
