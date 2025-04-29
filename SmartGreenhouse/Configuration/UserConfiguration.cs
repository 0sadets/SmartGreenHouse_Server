using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Configuration
{
    public class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            //builder.HasKey(u => u.Id);
            //builder.Property(u => u.UserName)
            //    .IsRequired()
            //    .HasMaxLength(100);
            //builder.Property(u => u.Email)
            //    .IsRequired()
            //    .HasMaxLength(150);
            //builder.Property(u => u.PasswordHash).IsRequired();
            builder.HasMany(u => u.Greenhouses)
              .WithOne(g => g.User)
              .HasForeignKey(g => g.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserSettings)
                   .WithOne(us => us.User)
                   .HasForeignKey(us => us.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
