using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models.Readers
{
    public class Reader
    {
        public string Name { get; set; }
        public List<string> ReadedBooks { get; set; }
    }
}
