using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.API.Data.Entities;

namespace User.API.Data.EntityConfigurations
{
    public class UsersEntityConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(u => u.NormalizedUserName);


            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.NormalizedEmail);
            builder.Property(u => u.EmailConfirmed);


            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(u => u.SecurityStamp);
            builder.Property(u => u.ConcurrencyStamp);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);
            builder.Property(u => u.PhoneNumberConfirmed);

            builder.Property(u => u.TwoFactorEnabled);

            builder.Property(u => u.LockoutEnd);
            builder.Property(u => u.LockoutEnabled);

            builder.Property(u => u.AccessFailedCount);

            builder.Property(u => u.Nickname)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(u => u.Description)
                .HasMaxLength(200);
            builder.Property(u => u.Role)
                .HasMaxLength(20);
            builder.Property(u => u.PhotoUrl)
                .HasMaxLength(200);
            builder.Property(u => u.RegisterDate)
                .IsRequired();
        }
    }
}