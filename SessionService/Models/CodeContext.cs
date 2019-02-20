using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace SessionService.Models
{
    public class CodesContext : DbContext
    {
        public DbSet<Code> Codes { get; set; }
        public CodesContext(DbContextOptions<CodesContext> options)
            : base(options)
        { }

        public CodesContext()
        { }
    }
}
