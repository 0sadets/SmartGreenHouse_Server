using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartGreenhouse.Models.Entities;
using System.Reflection.Emit;

namespace SmartGreenhouse.Configuration
{
    public class PlantConfiguration : IEntityTypeConfiguration<Plant>
    {
        public void Configure(EntityTypeBuilder<Plant> builder)
        {
            builder.ToTable("Plants");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Category)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(p=>p.ExampleNames)
                    .HasMaxLength(500);
      
        }
    }

}
