using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduPlatform.API.Services;
using EduPlatform.API.Models;
using System.Security.Claims;

namespace EduPlatform.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<ActionResult<Profile>> GetProfile()
        {
            try
            {
                var userId = GetCurrentUserId() ?? throw new Exception("User not authenticated");
                var profile = await _profileService.GetProfileAsync(userId.Value);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<Profile>> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var userId = GetCurrentUserId() ?? throw new Exception("User not authenticated");
                var profile = await _profileService.UpdateProfileAsync(userId.Value, request.FullName);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return null;
        }
    }

    public class UpdateProfileRequest
    {
        public string FullName { get; set; } = string.Empty;
    }
}
