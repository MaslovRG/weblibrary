using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionService.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirityAccess { get; set; }
        public DateTime ExpirityRefresh { get; set; }
        public string User { get; set; }

        public Token()
        {
            // empty
        }

        public Token(User user)
        {
            AccessToken = GetAT(user);
            RefreshToken = GetRT(user);
            ExpirityAccess = DateTime.Now.AddMinutes(30);
            ExpirityRefresh = DateTime.Now.AddMonths(1);
            User = user.Username; 
        }

        private static string GetAT(User user)
        {
            var at = user.Username + user.Password + DateTime.Now.ToString();
            return SHAConverter.GetHash(at); 
        }

        private static string GetRT(User user)
        {
            var rt = user.Password + DateTime.Now.ToString() + user.Username;
            return SHAConverter.GetHash(rt); 
        }
    }
}
