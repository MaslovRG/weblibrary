using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis; 

namespace BookService.Models
{
    [ExcludeFromCodeCoverage]
    public class Book
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
        public string Author { get; set; }
    }
}
