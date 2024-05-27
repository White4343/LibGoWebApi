using System.Collections;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Identity.API.Data
{
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<int>>
    {
        public ApplicationClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager,
            roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            ICollection<Claim> claims = new HashSet<Claim>(new ClaimComparer())
            {
                new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nickname),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim("Role", user.Role),
            };

            ClaimsIdentity identity =
                new ClaimsIdentity(claims, /*Explicit*/CookieAuthenticationDefaults.AuthenticationScheme);

            return identity;
        }
    }
}
