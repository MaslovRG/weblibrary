using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json; 

namespace BookService.Models
{
    public class Book
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
    }
}
