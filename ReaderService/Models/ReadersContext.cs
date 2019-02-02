﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 

namespace ReaderService.Models
{
    public class ReadersContext : IdentityDbContext
    {
        public DbSet<ReadedBook> ReadedBooks { get; set; }
        public DbSet<Reader> Readers { get; set; }        

        public ReadersContext(DbContextOptions<ReadersContext> options)
            : base(options)
        {
            Database.EnsureCreated(); 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Reader>()
                .HasMany(r => r.Books)
                .WithOne(e => e.Reader); 
        }
    }
}