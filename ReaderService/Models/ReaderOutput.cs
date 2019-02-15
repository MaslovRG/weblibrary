using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReaderService.Models
{
    public class ReaderOutput
    {
        public string Nickname { get; set; }
        public List<string> Books { get; set; }
    }
}
