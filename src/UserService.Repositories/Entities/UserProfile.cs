namespace UserService.Repositories.Entities;

/// <summary>
/// Alumni/Student detailed profile information stored in FAlumni_User_Db.
/// </summary>
public class UserProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string AvatarUrl { get; set; } = "https://cdn.falumni.local/avatars/default.jpg";

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

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
}
