using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Services.DTOs.Common;
using UserService.Services.DTOs.Requests;
using UserService.Services.DTOs.Responses;
using UserService.Services.Interfaces;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserProfileAppService _userProfileService;

    public UsersController(IUserProfileAppService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    /// <summary>
    /// US-09 &amp; AD-02: Search and filter alumni directory with pagination.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<UserProfileResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<UserProfileResponse>>> GetDirectory(
        [FromQuery] string? search = null,
        [FromQuery] int? graduationYear = null,
        [FromQuery] string? major = null,
        [FromQuery] string? company = null,
        [FromQuery] string? status = null,
        [FromQuery] string? role = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _userProfileService.GetDirectoryPagedAsync(
            search,
            graduationYear,
            major,
            company,
            status,
            role,
            pageNumber,
            pageSize,
            cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// US-09 Alias: Search alumni directory.
    /// </summary>
    [HttpGet("directory")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<UserProfileResponse>), StatusCodes.Status200OK)]
    public Task<ActionResult<PagedResponse<UserProfileResponse>>> GetDirectoryAlias(
        [FromQuery] string? search = null,
        [FromQuery] int? graduationYear = null,
        [FromQuery] string? major = null,
        [FromQuery] string? company = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return GetDirectory(search, graduationYear, major, company, "Active", null, pageNumber, pageSize, cancellationToken);
    }

    /// <summary>
    /// US-05: Get current authenticated user's profile (extracted strictly from JWT claims).
    /// </summary>
    [HttpGet("me")]
    [Authorize(Policy = "RequireUserRole")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserProfileResponse>> GetMyProfile(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var profile = await _userProfileService.GetByIdAsync(userId, cancellationToken);
        return Ok(profile);
    }

    /// <summary>
    /// US-06: Update current authenticated user's profile.
    /// </summary>
    [HttpPut("me")]
    [Authorize(Policy = "RequireUserRole")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserProfileResponse>> UpdateMyProfile(
        [FromBody] UpdateUserProfileRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var updated = await _userProfileService.UpdateProfileAsync(userId, request, cancellationToken);
        return Ok(updated);
    }

    /// <summary>
    /// US-07: Update current authenticated user's avatar URL.
    /// </summary>
    [HttpPut("me/avatar")]
    [Authorize(Policy = "RequireUserRole")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserProfileResponse>> UpdateMyAvatar(
        [FromBody] UpdateAvatarRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var updated = await _userProfileService.UpdateAvatarAsync(userId, request, cancellationToken);
        return Ok(updated);
    }

    /// <summary>
    /// US-08: Get public profile of a user by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var profile = await _userProfileService.GetByIdAsync(id, cancellationToken);
        return Ok(profile);
    }

    /// <summary>
    /// AD-03: Authorized endpoint for Admin / Admin Service to suspend a user account.
    /// </summary>
    [HttpPost("{id:guid}/suspend")]
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileResponse>> SuspendUser(
        Guid id,
        [FromBody] SuspendUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _userProfileService.SuspendUserAsync(id, request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// AD-04: Authorized endpoint for Admin / Admin Service to unsuspend a user account.
    /// </summary>
    [HttpPost("{id:guid}/unsuspend")]
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileResponse>> UnsuspendUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _userProfileService.UnsuspendUserAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// US-10: Update user role (USER / ADMIN) and account type (Student / Alumni / Admin).
    /// </summary>
    [HttpPut("{id:guid}/role")]
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileResponse>> UpdateRole(
        Guid id,
        [FromBody] UpdateUserRoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _userProfileService.UpdateUserRoleAsync(id, request, cancellationToken);
        return Ok(result);
    }

    private bool TryGetCurrentUserId(out Guid userId)
    {
        var claimVal = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst("sub")?.Value;
        return Guid.TryParse(claimVal, out userId);
    }
}
