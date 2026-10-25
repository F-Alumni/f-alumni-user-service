using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Repositories.Entities;

namespace UserService.Repositories.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.AvatarUrl)
            .HasMaxLength(500)
            .HasDefaultValue("https://cdn.falumni.local/avatars/default.jpg");

        builder.Property(x => x.StudentCode)
            .HasMaxLength(30);

        builder.Property(x => x.Major)
            .HasMaxLength(100);

        builder.Property(x => x.CurrentCompany)
            .HasMaxLength(150);

        builder.Property(x => x.JobTitle)
            .HasMaxLength(150);

        builder.Property(x => x.Bio)
            .HasMaxLength(1000);

        builder.Property(x => x.Skills)
            .HasMaxLength(500);

        builder.Property(x => x.LinkedInUrl)
            .HasMaxLength(300);

        builder.Property(x => x.GitHubUrl)
            .HasMaxLength(300);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(30);
    }
}
