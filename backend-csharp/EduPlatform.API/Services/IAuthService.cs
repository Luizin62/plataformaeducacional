using EduPlatform.API.DTOs;

namespace EduPlatform.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<UserDto> GetUserProfileAsync(Guid userId);
    }
}
