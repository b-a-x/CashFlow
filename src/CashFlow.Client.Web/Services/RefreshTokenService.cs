using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace CashFlow.Client.Web.Services
{
    public class RefreshTokenService
    {
        private readonly AuthenticationStateProvider _authProvider;
        private readonly IAuthenticationService _authService;

        public RefreshTokenService(AuthenticationStateProvider authProvider, IAuthenticationService authService)
        {
            _authProvider = authProvider;
            _authService = authService;
        }

        public async Task<string> TryRefreshToken()
        {
            AuthenticationState authState = await _authProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal user = authState.User;
            
            string exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            if (string.IsNullOrEmpty(exp) == false)
            {
                DateTimeOffset expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
                if ((expTime - DateTime.UtcNow).TotalMinutes <= 1)
                {
                    return await _authService.RefreshToken();
                }
            }

            return string.Empty;
        }
    }
}
