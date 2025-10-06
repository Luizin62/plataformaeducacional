using EduPlatform.API.Models;

namespace EduPlatform.API.Services
{
    public interface IProfileService
    {
        Task<Profile> GetProfileAsync(Guid userId);
        Task<Profile> UpdateProfileAsync(Guid userId, string fullName);
    }
}
