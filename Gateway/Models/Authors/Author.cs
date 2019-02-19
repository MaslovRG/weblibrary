using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 

namespace Gateway.Models.Authors
{
    public class Author
    {
        [Required]
        [Display(Name = "Name")]
        [DataType(DataType.Text, ErrorMessage = "Input author name")]
        [RegularExpression(@"[a-zA-Z0-9 ]+", ErrorMessage = "Input correct author name (Only spaces, letters and numbers are allowed)")]
        public string Name { get; set; }
    }
}
