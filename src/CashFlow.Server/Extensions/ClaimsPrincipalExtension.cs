using System.Security.Claims;

namespace CashFlow.Server.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetIdentifier(this ClaimsPrincipal claims) => 
            claims.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
