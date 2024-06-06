using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.API.Data.Entities;

namespace User.API.Data.EntityConfigurations
{
    public class BoughtBooksEntityConfiguration : IEntityTypeConfiguration<BoughtBooks>
    {
        public void Configure(EntityTypeBuilder<BoughtBooks> builder)
        {
            builder.ToTable("BoughtBooks");

            builder.HasKey(bb => bb.Id);

            builder.Property(bb => bb.PurchaseDate)
                .IsRequired();

            builder.Property(bb => bb.Price)
                .IsRequired();

            builder.Property(bb => bb.IsPaidToAuthor)
                .IsRequired();

            builder.Property(bb => bb.SubscriptionId);

            builder.Property(bb => bb.AuthorUserId)
                .IsRequired();

            builder.Property(bb => bb.UserId)
                .IsRequired();

            builder.Property(bb => bb.BookId)
                .IsRequired();

            builder.Property(bb => bb.BookName)
                .IsRequired();

            builder.Property(bb => bb.BookPhoto);
        }
    }
}
