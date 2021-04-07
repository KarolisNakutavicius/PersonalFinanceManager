using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Service.Helpers
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this ClaimsIdentity identity)
        {

            if (!identity.IsAuthenticated)
                throw new AuthenticationException();

            string userId = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return userId;
        }
    }
}
