using System.Security.Cryptography;
using UserService.Repositories.Entities;
using UserService.Services.DTOs.Responses;

namespace UserService.Services.Implementations;

public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);

        return $"PBKDF2${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        if (string.IsNullOrWhiteSpace(storedHash))
        {
            return false;
        }

        var parts = storedHash.Split('$');
        if (parts.Length != 4 || parts[0] != "PBKDF2")
        {
            return false;
        }

        if (!int.TryParse(parts[1], out var iterations))
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[2]);
        var expectedHash = Convert.FromBase64String(parts[3]);

        var actualHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}

public static class UserMappingExtensions
{
    public static UserProfileResponse ToResponse(this User user)
    {
        var p = user.Profile;
        return new UserProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            DisplayName = !string.IsNullOrWhiteSpace(p?.DisplayName) ? p.DisplayName : user.FullName,
            AvatarUrl = !string.IsNullOrWhiteSpace(p?.AvatarUrl) ? p.AvatarUrl : "https://cdn.falumni.local/avatars/default.jpg",
            Role = user.Role,
            AccountType = user.AccountType,
            Status = user.Status,
            EmailVerified = user.EmailVerified,
            StudentCode = p?.StudentCode ?? string.Empty,
            GraduationYear = p?.GraduationYear,
            Major = p?.Major ?? string.Empty,
            CurrentCompany = p?.CurrentCompany ?? string.Empty,
            JobTitle = p?.JobTitle ?? string.Empty,
            Bio = p?.Bio ?? string.Empty,
            Skills = p?.Skills ?? string.Empty,
            LinkedInUrl = p?.LinkedInUrl ?? string.Empty,
            GitHubUrl = p?.GitHubUrl ?? string.Empty,
            PhoneNumber = p?.PhoneNumber ?? string.Empty,
            CreatedAtUtc = user.CreatedAtUtc,
            UpdatedAtUtc = user.UpdatedAtUtc
        };
    }
}
