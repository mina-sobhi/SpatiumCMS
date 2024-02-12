using Domain.ApplicationUserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntitiesConfigurations
{
    public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasMany(x => x.ApplicationUsers)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId);

            builder.HasOne(x => x.RoleOwner)
                   .WithMany(x => x.OwnedRoles)
                   .HasForeignKey(x => x.RoleOwnerId);
        }
    }
}
