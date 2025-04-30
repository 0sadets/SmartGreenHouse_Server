using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Configuration
{
    public class SensorReadingConfiguration : IEntityTypeConfiguration<SensorReading>
    {
        public void Configure(EntityTypeBuilder<SensorReading> builder)
        {
            builder.ToTable("SensorReadings");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Timestamp)
                   .IsRequired();

            builder.HasOne(s => s.Greenhouse)
                   .WithMany(g => g.SensorReadings)
                   .HasForeignKey(s => s.GreenhouseId);
        }
    }

}
