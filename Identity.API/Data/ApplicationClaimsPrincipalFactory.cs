using System.Security.Claims;
using IdentityModel;
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
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim(JwtClaimTypes.Subject, user.Id.ToString()));
            identity.AddClaim(new Claim(JwtClaimTypes.PreferredUserName, user.Nickname));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
            identity.AddClaim(new Claim("Role", user.Role));

            return identity;
        }
    }
}
