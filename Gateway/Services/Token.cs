using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class Token
    {
        public string Value { get; set; }
        public DateTime Expirity { get; set; }
    }
}
