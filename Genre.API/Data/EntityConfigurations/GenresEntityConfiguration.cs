using Genre.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Genre.API.Data.EntityConfigurations
{
    public class GenresEntityConfiguration : IEntityTypeConfiguration<Genres>
    {
        public void Configure(EntityTypeBuilder<Genres> builder)
        {
            builder.ToTable("Genres");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .UseHiLo("genres_hilo")
                .IsRequired();

            builder.Property(g => g.Name)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
