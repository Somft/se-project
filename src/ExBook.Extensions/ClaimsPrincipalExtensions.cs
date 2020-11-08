using System;
using System.Linq;
using System.Security.Claims;

namespace ExBook.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetClaim(this ClaimsPrincipal principal, string claim)
        {
            return principal.Claims.FirstOrDefault(c => c.Type == claim)?.Value;
        }

        public static Guid? GetId(this ClaimsPrincipal principal)
        {
            string? idClaim = principal.GetClaim(ClaimTypes.Sid);

            if (idClaim != null && Guid.TryParse(idClaim, out Guid result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
