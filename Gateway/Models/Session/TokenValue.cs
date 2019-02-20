using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models.Session
{
    public class TokenValue
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirityAccess { get; set; }
        public DateTime ExpiritRefresh { get; set; }
    }
}
