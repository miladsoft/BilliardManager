using Billiard.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billiard.DataLayer.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.ToTable("AppUsers");
    }
}