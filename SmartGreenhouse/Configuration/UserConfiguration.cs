using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Configuration
{
    public class UserConfiguration: IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Users");
            
            builder.HasMany(u => u.Greenhouses)
              .WithOne(g => g.User)
              .HasForeignKey(g => g.UserId);

            builder.HasMany(u => u.UserSettings)
                   .WithOne(us => us.User)
                   .HasForeignKey(us => us.UserId);
        }
    }
}
