using EduPlatform.API.DTOs;
using EduPlatform.API.Models;

namespace EduPlatform.API.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync(Guid? userId = null);
        Task<CourseResponseDto> GetCourseByIdAsync(Guid courseId, Guid? userId = null);
        Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto dto, Guid instructorId);
        Task<CourseResponseDto> UpdateCourseAsync(Guid courseId, UpdateCourseDto dto);
        Task<bool> DeleteCourseAsync(Guid courseId);
        Task<IEnumerable<LessonResponseDto>> GetCourseLessonsAsync(Guid courseId, Guid? userId = null);
        Task<LessonResponseDto> CreateLessonAsync(Guid courseId, CreateLessonDto dto);
    }
}
