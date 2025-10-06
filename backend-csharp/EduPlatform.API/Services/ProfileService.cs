using EduPlatform.API.Models;
using Supabase;

namespace EduPlatform.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly Client _supabase;

        public ProfileService(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<Profile> GetProfileAsync(Guid userId)
        {
            var profile = await _supabase.From<Profile>()
                .Where(p => p.Id == userId)
                .Single();

            return profile;
        }

        public async Task<Profile> UpdateProfileAsync(Guid userId, string fullName)
        {
            var profile = await _supabase.From<Profile>()
                .Where(p => p.Id == userId)
                .Single();

            profile.FullName = fullName;
            profile.UpdatedAt = DateTime.UtcNow;

            await _supabase.From<Profile>().Update(profile);

            return profile;
        }
    }
}
