using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 

namespace Gateway.Models.Books
{
    public class Book
    {
        [Required]
        [Display(Name = "Name")]
        [DataType(DataType.Text, ErrorMessage = "Input book name")]
        [RegularExpression(@"[a-zA-Z0-9 ]+", ErrorMessage = "Input correct book name (Only spaces, letters and numbers are allowed)")]
        public string Name { get; set; }
        [Range(0, 2040, ErrorMessage ="Input correct year (from 0)")]
        public int? Year { get; set; }
        [DataType(DataType.Text, ErrorMessage = "Input author name")]
        [RegularExpression(@"[a-zA-Z0-9 ]+", ErrorMessage = "Input correct author name (Only spaces, letters and numbers are allowed)")]
        public string Author { get; set; }
    }
}
