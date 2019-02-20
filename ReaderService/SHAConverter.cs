using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace ReaderService
{
    public static class SHAConverter
    {
        public static string GetHash(string Password)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var temp = sha1.ComputeHash(Encoding.Unicode.GetBytes(Password));
            return Convert.ToBase64String(temp); 
        }
    }
}
