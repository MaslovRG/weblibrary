﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Models.Session;

namespace Gateway.Services
{
    public interface ISessionService
    {
        Task<Result<TokenValue>> GetToken(User user);
        Task<Result> CheckToken(string AccessToken);
        Task<Result<TokenValue>> Refresh(string RefreshToken); 
    }
}
