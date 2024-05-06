using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;

namespace Identity.API.Data
{
    public class ApplicationResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationSignInManager _signInManager;

        public ApplicationResourceOwnerPasswordValidator(ApplicationDbContext context, ApplicationSignInManager signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var result = await _signInManager.PasswordSignInAsync(context.UserName, context.Password, false, false);

            if (result.Succeeded)
            {
                context.Result = new GrantValidationResult(_context.Users.Single(u => u.UserName == context.UserName).Id.ToString(), "password");
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            }
        }
    }
}
