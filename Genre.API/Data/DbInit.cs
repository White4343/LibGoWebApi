namespace Genre.API.Data
{
    public class DbInit
    {
        private static IEnumerable<Genres> GetPreconfiguredGenres()
        {
            return new List<Genres>
            {
                new Genres { Name = "Science Fiction" },
                new Genres { Name = "Fantasy" },
                new Genres { Name = "Mystery" },
                new Genres { Name = "Thriller" },
                new Genres { Name = "Romance" },
                new Genres { Name = "Western" },
                new Genres { Name = "Dystopian" },
                new Genres { Name = "Contemporary" },
                new Genres { Name = "Horror" },
                new Genres { Name = "Literary Fiction" }
            };
        }

        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.Genres.AnyAsync())
            {
                await context.Genres.AddRangeAsync(GetPreconfiguredGenres());
                await context.SaveChangesAsync();
            }
        }
    }
}