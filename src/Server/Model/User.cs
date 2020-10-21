using System;
using Microsoft.AspNetCore.Identity;

namespace CashFlowManagement.Server.Model
{
    public class User : IdentityUser
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
