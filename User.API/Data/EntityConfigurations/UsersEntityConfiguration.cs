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
            builder.Property(u => u.Login) 
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Nickname)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(u => u.Description)
                .HasMaxLength(200);
        }
    }
}
