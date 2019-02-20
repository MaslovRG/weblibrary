using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace SessionService.Models
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        [ExcludeFromCodeCoverage]
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
            Initalize(); 
        }
        
        public UsersContext()
        { }

        private void Initalize()
        {
            if (!Users.Any())
            {                 
                Users.Add(new User { Username = "M", Password =  SHAConverter.GetHash("111")});
                Users.Add(new User { Username = "L", Password = SHAConverter.GetHash("222") });
                Users.Add(new User { Username = "O", Password = SHAConverter.GetHash("333") });
                SaveChanges(); 
            }
        }
    }
}
