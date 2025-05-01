using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Configuration
{
    public class GreenhousePlantConfiguration : IEntityTypeConfiguration<GreenhousePlant>
    {
        public void Configure(EntityTypeBuilder<GreenhousePlant> builder)
        {
            builder.HasKey(gp => new { gp.GreenhouseId, gp.PlantId });
            builder.HasOne(gp => gp.Greenhouse)
                .WithMany(g => g.GreenhousePlants)
                .HasForeignKey(gp => gp.GreenhouseId);

            builder.HasOne(gp => gp.Plant)
                .WithMany(p => p.GreenhousePlants)
                .HasForeignKey(gp => gp.PlantId);
        }
    }
}
