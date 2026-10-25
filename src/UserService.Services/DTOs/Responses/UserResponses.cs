namespace UserService.Services.DTOs.Responses;

public class RegisterResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// Included in dev/demo mode to allow quick testing without SMTP server.
    /// </summary>
    public string? VerificationOtpPreview { get; set; }
}

public class AuthTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; } = 3600;
    public UserProfileResponse User { get; set; } = new();
}

public class UserProfileResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Role { get; set; } = "USER";
    public string AccountType { get; set; } = "Alumni";
    public string Status { get; set; } = "Active";
    public bool EmailVerified { get; set; }
    public string StudentCode { get; set; } = string.Empty;
    public int? GraduationYear { get; set; }
    public string Major { get; set; } = string.Empty;
    public string CurrentCompany { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string LinkedInUrl { get; set; } = string.Empty;
    public string GitHubUrl { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
