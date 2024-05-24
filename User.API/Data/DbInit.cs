using Microsoft.EntityFrameworkCore;
using User.API.Data.Entities;

namespace User.API.Data
{
    public class DbInit
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.EnsureCreatedAsync();
            
            await context.SaveChangesAsync();
        }
    }
}
