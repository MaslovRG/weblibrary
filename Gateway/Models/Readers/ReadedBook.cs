using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 

namespace Gateway.Models.Readers
{
    public class ReadedBook
    {
        [Display(Name="Book")]
        public string Name { get; set; }
        [Display(Name="Reader")]
        public string Nickname { get; set; }
    }
}
