using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.API.Data.Entities;

namespace User.API.Data.EntityConfigurations
{
    public class SubscriptionsEntityConfiguration : IEntityTypeConfiguration<Subscriptions>
    {
        public void Configure(EntityTypeBuilder<Subscriptions> builder)
        {
            builder.ToTable("Subscriptions");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.IsActive)
                .IsRequired();

            builder.Property(s => s.Name)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(s => s.Description)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(s => s.Price)
                .IsRequired();

            builder.Property(s => s.PublishDate)
                .IsRequired();

            builder.Property(s => s.UpdateDate);

            builder.Property(s => s.BookIds)
                .IsRequired();

            builder.Property(s => s.UserId)
                .IsRequired();
        }
    }
}