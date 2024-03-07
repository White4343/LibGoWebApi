using Book.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.EntityConfigurations
{
    public class BooksEntityConfiguration : IEntityTypeConfiguration<Books>
    {
        public void Configure(EntityTypeBuilder<Books> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .UseHiLo("books_hilo")
                .IsRequired();

            builder.Property(b => b.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(b => b.Description)
                .HasMaxLength(400)
                .IsRequired();

            builder.Property(b => b.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(b => b.PublishDate)
                .IsRequired();

            builder.Property(b => b.UserId)
                .IsRequired();

            builder.Property(b => b.CoAuthorIds);
        }
    }
}