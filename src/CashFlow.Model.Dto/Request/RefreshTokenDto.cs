using System;
using System.Collections.Generic;
using System.Text;

namespace CashFlow.Model.Dto.Request
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
