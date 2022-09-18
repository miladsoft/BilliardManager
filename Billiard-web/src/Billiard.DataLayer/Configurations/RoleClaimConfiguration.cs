using Billiard.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billiard.DataLayer.Mappings
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.HasOne(roleClaim => roleClaim.Role)
                   .WithMany(role => role.Claims)
                   .HasForeignKey(roleClaim => roleClaim.RoleId);

            builder.ToTable("AppRoleClaims");
        }
    }
}