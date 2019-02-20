using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionService.Models
{
    public class TokenValue
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirityAccess { get; set; }
        public DateTime ExpiritRefresh { get; set; }

        public TokenValue()
        {
            // empty
        }

        public TokenValue(Token token)
        {
            AccessToken = token.AccessToken;
            RefreshToken = token.RefreshToken;
            ExpirityAccess = token.ExpirityAccess;
            ExpiritRefresh = token.ExpirityRefresh; 
        }
    }
}
