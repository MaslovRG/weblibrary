using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 

namespace Gateway.Models.Readers
{
    public class Reader
    {
        [Required]
        [Display(Name = "Nickname")]
        [DataType(DataType.Text, ErrorMessage = "Input author name")]
        [RegularExpression(@"[a-zA-Z0-9]+", ErrorMessage = "Input correct nickname (Only letters and numbers are allowed)")]
        public string Nickname { get; set; }
        public List<string> Books { get; set; }
    }
}
