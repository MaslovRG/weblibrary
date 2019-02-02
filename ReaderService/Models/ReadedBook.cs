using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReaderService.Models
{
    public class ReadedBook
    {
        public int Id { get; set; }
        public string Name { get; set; } 

        public Reader Reader { get; set; }
    }
}
