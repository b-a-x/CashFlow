using System;
using Microsoft.AspNetCore.Identity;

namespace CashFlow.DataProvider.EFCore.Model
{
    public class User : IdentityUser
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
