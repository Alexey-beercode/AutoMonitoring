using AutoMonitoring.Domain.Entities.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoMonitoring.DAL.Infrastructure.Database.Configuration;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessions");
        builder.HasKey(us => us.Id);

        builder.Property(us => us.UserId)
            .IsRequired();

        builder.Property(us => us.DeviceName)
            .IsRequired();

        builder.Property(us => us.RefreshToken)
            .IsRequired();

        builder.Property(us => us.RefreshTokenExpiryTime)
            .IsRequired();

        builder.Property(us => us.IsActive)
            .IsRequired();
    }
}
