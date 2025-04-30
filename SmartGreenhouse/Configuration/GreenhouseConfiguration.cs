using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Configuration
{
    public class GreenhouseConfiguration : IEntityTypeConfiguration<Greenhouse>
    {
        public void Configure(EntityTypeBuilder<Greenhouse> builder)
        {
            builder.ToTable("Greenhouses");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(g => g.Season)
                   .HasMaxLength(50);

            builder.Property(g => g.Location)
                   .HasMaxLength(200);

            builder.HasOne(g => g.User)
                   .WithMany(u => u.Greenhouses)
                   .HasForeignKey(g => g.UserId);

            builder.HasMany(g => g.SensorReadings)
                   .WithOne(s => s.Greenhouse)
                   .HasForeignKey(s => s.GreenhouseId);

            builder.HasMany(g => g.DeviceStates)
                   .WithOne(d => d.Greenhouse)
                   .HasForeignKey(d => d.GreenhouseId);

            builder.HasMany(g => g.UserSettings)
                   .WithOne(us => us.Greenhouse)
                   .HasForeignKey(us => us.GreenhouseId);
        }
    }
}
