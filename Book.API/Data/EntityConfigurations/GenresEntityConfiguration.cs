using Book.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.EntityConfigurations
{
    public class GenresEntityConfiguration : IEntityTypeConfiguration<Genres>
    {
        public void Configure(EntityTypeBuilder<Genres> builder)
        {
            builder.ToTable("Genres");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .UseHiLo()
                .IsRequired();

            builder.Property(g => g.Name)
                .IsRequired();
        }
    }
}
