using Book.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.EntityConfigurations
{
    public class CommentsEntityConfiguration : IEntityTypeConfiguration<Comments>
    {
        public void Configure(EntityTypeBuilder<Comments> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .UseHiLo("comments_hilo")
                .IsRequired();

            builder.Property(c => c.Content)
                .HasMaxLength(400)
                .IsRequired();

            builder.Property(c => c.UserNickname)
                .IsRequired();

            builder.Property(c => c.UserPhotoUrl);

            builder.Property(c => c.CreateDate)
                .IsRequired();

            builder.Property(c => c.UpdateDate);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.HasOne(c => c.Book)
                .WithMany()
                .HasForeignKey(c => c.BookId)
                .IsRequired();
        }
    }
}