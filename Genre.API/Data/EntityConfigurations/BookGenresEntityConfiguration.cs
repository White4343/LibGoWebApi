using Genre.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Genre.API.Data.EntityConfigurations
{
    public class BookGenresEntityConfiguration : IEntityTypeConfiguration<BookGenres>
    {
        public void Configure(EntityTypeBuilder<BookGenres> builder)
        {
            builder.ToTable("BookGenres");

            builder.HasKey(bg => bg.Id);

            builder.Property(bg => bg.Id)
                .UseHiLo("bookgenres_hilo")
                .IsRequired();

            builder.Property(bg => bg.BookId)
                .IsRequired();

            builder.HasOne(bg => bg.Genre)
                .WithMany()
                .HasForeignKey(bg => bg.GenreId)
                .IsRequired();
        }
    }
}
