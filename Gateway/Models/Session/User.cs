using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 

namespace Gateway.Models.Session
{
    public class User
    {
        [Required]
        [DataType(DataType.Text, ErrorMessage = "Input username")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Text, ErrorMessage = "Input password")]
        public string Password { get; set; }
    }
}
