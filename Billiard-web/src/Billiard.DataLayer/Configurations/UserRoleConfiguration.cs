using Billiard.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billiard.DataLayer.Mappings
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasOne(userRole => userRole.Role)
                   .WithMany(role => role.Users)
                   .HasForeignKey(userRole => userRole.RoleId);

            builder.HasOne(userRole => userRole.User)
                   .WithMany(user => user.Roles)
                   .HasForeignKey(userRole => userRole.UserId);

            builder.ToTable("AppUserRoles");
        }
    }
}