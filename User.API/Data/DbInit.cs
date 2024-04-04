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
                new Users {Login = "User1", Password = "123", Email = "email1@email.com", Nickname = "Nickname1", Description = "..."},
                new Users {Login = "User2", Password = "456", Email = "email2@email.com", Nickname = "Nickname2", Description = "..."},
                new Users {Login = "User3", Password = "789", Email = "email3@email.com", Nickname = "Nickname3", Description = "..."},
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
