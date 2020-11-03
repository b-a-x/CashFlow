using System;
using System.Collections.Generic;
using System.Text;

namespace CashFlow.Model.Dto.Response
{
    public class RegistrationResponseDto
    {
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
