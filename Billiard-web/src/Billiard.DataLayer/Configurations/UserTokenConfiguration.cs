using Billiard.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billiard.DataLayer.Mappings
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.HasOne(userToken => userToken.User)
                   .WithMany(user => user.UserTokens)
                   .HasForeignKey(userToken => userToken.UserId);

            builder.ToTable("AppUserTokens");
        }
    }
}