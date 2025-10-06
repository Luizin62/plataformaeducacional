using EduPlatform.API.DTOs;
using EduPlatform.API.Models;
using Supabase;

namespace EduPlatform.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly Client _supabase;

        public AuthService(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var authResponse = await _supabase.Auth.SignUp(dto.Email, dto.Password);

            if (authResponse?.User == null)
                throw new Exception("Registration failed");

            var profile = new Profile
            {
                Id = Guid.Parse(authResponse.User.Id),
                FullName = dto.FullName,
                Role = "student",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _supabase.From<Profile>().Insert(profile);

            return new AuthResponseDto
            {
                AccessToken = authResponse.AccessToken ?? string.Empty,
                RefreshToken = authResponse.RefreshToken ?? string.Empty,
                User = new UserDto
                {
                    Id = profile.Id,
                    Email = dto.Email,
                    FullName = dto.FullName,
                    Role = "student"
                }
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var authResponse = await _supabase.Auth.SignIn(dto.Email, dto.Password);

            if (authResponse?.User == null)
                throw new Exception("Login failed");

            var userId = Guid.Parse(authResponse.User.Id);
            var profile = await _supabase.From<Profile>()
                .Where(p => p.Id == userId)
                .Single();

            return new AuthResponseDto
            {
                AccessToken = authResponse.AccessToken ?? string.Empty,
                RefreshToken = authResponse.RefreshToken ?? string.Empty,
                User = new UserDto
                {
                    Id = profile.Id,
                    Email = dto.Email,
                    FullName = profile.FullName,
                    Role = profile.Role
                }
            };
        }

        public async Task<UserDto> GetUserProfileAsync(Guid userId)
        {
            var profile = await _supabase.From<Profile>()
                .Where(p => p.Id == userId)
                .Single();

            var user = _supabase.Auth.CurrentUser;

            return new UserDto
            {
                Id = profile.Id,
                Email = user?.Email ?? string.Empty,
                FullName = profile.FullName,
                Role = profile.Role
            };
        }
    }
}
