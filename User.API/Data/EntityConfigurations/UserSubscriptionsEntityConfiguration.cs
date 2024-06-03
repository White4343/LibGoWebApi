using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.API.Data.Entities;

namespace User.API.Data.EntityConfigurations
{
    public class UserSubscriptionsEntityConfiguration : IEntityTypeConfiguration<UserSubscriptions>
    {
        public void Configure(EntityTypeBuilder<UserSubscriptions> builder)
        {
            builder.ToTable("UserSubscriptions");

            builder.HasKey(us => us.Id);

            builder.Property(us => us.StartDate)
                .IsRequired();

            builder.Property(us => us.EndDate)
                .IsRequired();

            builder.Property(us => us.PaidPrice)
                .IsRequired();

            builder.Property(us => us.IsPaidToAuthor)
                .IsRequired();

            builder.Property(us => us.UserId)
                .IsRequired();

            builder.Property(us => us.SubscriptionId)
                .IsRequired();

            builder.Property(us => us.BookIds)
                .IsRequired();

            builder.Property(us => us.AuthorUserId)
                .IsRequired();
        }
    }
}
