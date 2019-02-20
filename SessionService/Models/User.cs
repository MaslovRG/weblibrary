using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace SessionService.Models
{
    [ExcludeFromCodeCoverage]
    public class User
    {
        [JsonIgnore]
        [ExcludeFromCodeCoverage]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
