using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Configuration
{
    public class UserSettingConfiguration : IEntityTypeConfiguration<UserSetting>
    {
        public void Configure(EntityTypeBuilder<UserSetting> builder)
        {
            builder.ToTable("UserSettings");

            builder.HasKey(us => us.Id);

            builder.HasOne(us => us.User)
                   .WithMany(u => u.UserSettings)
                   .HasForeignKey(us => us.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(us => us.Greenhouse)
                   .WithMany(g => g.UserSettings)
                   .HasForeignKey(us => us.GreenhouseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
