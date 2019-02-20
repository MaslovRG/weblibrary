using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionService.Models
{
    public class Code
    {
        public int Id { get; set; }
        public string CodeValue { get; set; }
        public string Username { get; set; }

        public Code()
        {

        }

        public Code(User user)
        {
            CodeValue = SHAConverter.GetHash(DateTime.Now.ToString() + user.Password + DateTime.Now.ToString());
            Username = user.Username; 
        }
    }
}
