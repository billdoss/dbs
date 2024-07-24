using Microsoft.IdentityModel.Tokens;

namespace dbs.Utils
{
    public static class SecurityUtils
    {
        public static bool TokenLifetimeValidate(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken tokenToValidate,
            TokenValidationParameters @param
        )
        {
            return (expires != null && expires > DateTime.UtcNow);
        }
    }


}
