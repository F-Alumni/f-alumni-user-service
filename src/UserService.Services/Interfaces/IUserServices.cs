using UserService.Repositories.Entities;
using UserService.Services.DTOs.Common;
using UserService.Services.DTOs.Requests;
using UserService.Services.DTOs.Responses;

namespace UserService.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthTokenResponse> VerifyEmailAsync(VerifyEmailRequest request, CancellationToken cancellationToken = default);
    Task<AuthTokenResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<AuthTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<RegisterResponse> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default);
    Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
}

public interface IUserProfileAppService
{
    Task<UserProfileResponse> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateUserProfileRequest request, CancellationToken cancellationToken = default);
    Task<UserProfileResponse> UpdateAvatarAsync(Guid userId, UpdateAvatarRequest request, CancellationToken cancellationToken = default);
    Task<PagedResponse<UserProfileResponse>> GetDirectoryPagedAsync(
        string? search,
        int? graduationYear,
        string? major,
        string? company,
        string? status,
        string? role,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<UserProfileResponse> SuspendUserAsync(Guid userId, SuspendUserRequest request, CancellationToken cancellationToken = default);
    Task<UserProfileResponse> UnsuspendUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserProfileResponse> UpdateUserRoleAsync(Guid userId, UpdateUserRoleRequest request, CancellationToken cancellationToken = default);
}
