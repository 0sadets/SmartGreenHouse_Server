using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Token).IsRequired();
            builder.Property(r => r.ExpiresAt).IsRequired();

            builder.HasOne(r => r.AppUser)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(r => r.AppUserId);
        }
    }

}
