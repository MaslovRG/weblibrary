using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace AuthorService.Models
{
    [ExcludeFromCodeCoverage]
    public class Author
    {
        [JsonIgnore]
        [ExcludeFromCodeCoverage]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
