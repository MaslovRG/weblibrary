using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis; 

namespace ReaderService.Models
{
    public class Reader
    {
        [ExcludeFromCodeCoverage]
        [JsonIgnore]
        public int Id { get; set; }
        public string Nickname { get; set; }

        public ICollection<ReadedBook> Books { get; set; }
    }
}
