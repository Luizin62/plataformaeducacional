using EduPlatform.API.Models;

namespace EduPlatform.API.Services
{
    public interface IEnrollmentService
    {
        Task<Enrollment> EnrollInCourseAsync(Guid studentId, Guid courseId);
        Task<IEnumerable<Enrollment>> GetStudentEnrollmentsAsync(Guid studentId);
        Task<bool> UpdateLessonProgressAsync(Guid enrollmentId, Guid lessonId, bool completed);
    }
}
