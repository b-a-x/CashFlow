using System.Collections.Generic;

namespace WebClient.Model.Dto
{
    public class RegistrationResponseDto
    {
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
