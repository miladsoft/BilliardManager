using Billiard.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billiard.DataLayer.Mappings
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.HasOne(userClaim => userClaim.User)
                   .WithMany(user => user.Claims)
                   .HasForeignKey(userClaim => userClaim.UserId);

            builder.ToTable("AppUserClaims");
        }
    }
}