using Chapter.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chapter.API.Data.EntityConfigurations
{
    public class ChaptersEntityConfiguration : IEntityTypeConfiguration<Chapters>
    {
        public void Configure(EntityTypeBuilder<Chapters> builder)
        {
            builder.ToTable("Chapters");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Content).IsRequired();
            builder.Property(c => c.IsFree).IsRequired();
            builder.Property(c => c.CreatedAt).IsRequired();
            builder.Property(c => c.UpdatedAt);
            builder.Property(c => c.BookId).IsRequired();
        }
    }
}