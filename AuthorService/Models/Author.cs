using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json; 

namespace AuthorService.Models
{
    public class Author
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
