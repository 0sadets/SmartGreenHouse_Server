using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Configuration
{
    public class DeviceStateConfiguration : IEntityTypeConfiguration<DeviceState>
    {
        public void Configure(EntityTypeBuilder<DeviceState> builder)
        {
            builder.ToTable("DeviceStates");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Timestamp)
                   .IsRequired();

            builder.HasOne(d => d.Greenhouse)
                   .WithMany(g => g.DeviceStates)
                   .HasForeignKey(d => d.GreenhouseId);
        }
    }

}
