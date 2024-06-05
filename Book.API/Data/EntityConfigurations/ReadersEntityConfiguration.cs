using Book.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.EntityConfigurations
{
    public class ReadersEntityConfiguration : IEntityTypeConfiguration<Readers>
    {
        public void Configure(EntityTypeBuilder<Readers> builder)
        {
            builder.ToTable("Readers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseHiLo("readers_hilo")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.NotifyEnabled)
                .IsRequired();

            builder.Property(x => x.IsVisible)
                .IsRequired();

            builder.Property(x => x.Rating);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasOne(x => x.Book)
                .WithMany()
                .HasForeignKey(x => x.BookId)
                .IsRequired();

            builder.Property(x => x.ChapterId);
        }
    }
}
