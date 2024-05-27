using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using System.Security.Claims;
using Duende.IdentityServer.Extensions;

namespace Identity.API.Data
{
    public class ApplicationProfileService : IProfileService
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationProfileService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            int userId = int.Parse(context.Subject.GetSubjectId()); 

            var user = _dbContext.Users.Find(userId);

            if (context.RequestedClaimTypes.Any())
            {
                context.AddRequestedClaims(new[] { new Claim("nickname", user.Nickname) });
                context.AddRequestedClaims(new[] { new Claim("role", user.Role) });
                context.AddRequestedClaims(new[] { new Claim("photoUrl", user.PhotoUrl) });
            }
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;

            return Task.CompletedTask;
        }
    }
}
