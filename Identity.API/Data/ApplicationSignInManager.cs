using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace Identity.API.Data
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser>
    {
        public ApplicationSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<ApplicationUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public override async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            ArgumentNullException.ThrowIfNull(user);

            var attempt = await CheckValidPasswordSignInAsync(user, password, lockoutOnFailure);
            return attempt.Succeeded
                ? await SignInOrTwoFactorAsync(user, isPersistent)
                : attempt;
        }

        private async Task<SignInResult> CheckValidPasswordSignInAsync(ApplicationUser user, string password,
            bool lockoutOnFailure)
        {
            if (lockoutOnFailure)
            {
                await UserManager.AccessFailedAsync(user);
                if (await UserManager.IsLockedOutAsync(user))
                {
                    return SignInResult.LockedOut;
                }
            }

            try
            {
                VerifyPassword(password, user.PasswordHash);

                return SignInResult.Success;
            }
            catch (ValidationException e)
            {
                Console.WriteLine(e);
                
                return SignInResult.Failed;
            }

            return SignInResult.Failed;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        private void VerifyPassword(string password, string passwordHash)
        {
            if (!BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash))
            {
                throw new ValidationException("Password is incorrect");
            }
        }
    }
}
