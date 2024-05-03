using Microsoft.EntityFrameworkCore;
using User.API.Data.Entities;

namespace User.API.Data
{
    public class DbInit
    {
        private static IEnumerable<Users> GetPreconfiguredUsers()
        {
            return new List<Users>
            {

            };
        }

        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.EnsureCreatedAsync();
            
            if (!await context.Users.AnyAsync())
            { 
                await context.Users.AddRangeAsync(GetPreconfiguredUsers());
            }

            await context.SaveChangesAsync();
        }
    }
}
