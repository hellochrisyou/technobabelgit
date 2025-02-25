﻿using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TBProj
{
    public interface IAuthenticator
    {
        Task<AuthenticationResult> Authenticate(string authority, string resource, string clientId, string returnUri);
        void Logout();
    }
}
