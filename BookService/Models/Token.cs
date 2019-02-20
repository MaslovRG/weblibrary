using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookService.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime Expirity { get; set; }

        public Token()
        {
            // empty
        }
    }
}
