using Book.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.EntityConfigurations
{
    public class BooksGenresEntityConfiguration : IEntityTypeConfiguration<BookGenres>
    {
        public void Configure(EntityTypeBuilder<BookGenres> builder)
        {
            builder.HasKey(bg => bg.Id);

            builder.HasOne(bg => bg.Book)
                .WithMany()
                .HasForeignKey(bg => bg.BookId);

            builder.HasOne(bg => bg.Genre)
                .WithMany()
                .HasForeignKey(bg => bg.GenreId);
        }
    }
}
