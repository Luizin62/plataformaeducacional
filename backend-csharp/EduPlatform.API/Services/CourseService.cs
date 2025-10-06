using EduPlatform.API.DTOs;
using EduPlatform.API.Models;
using Supabase;

namespace EduPlatform.API.Services
{
    public class CourseService : ICourseService
    {
        private readonly Client _supabase;

        public CourseService(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync(Guid? userId = null)
        {
            var courses = await _supabase.From<Course>()
                .Where(c => c.Published == true)
                .Get();

            var courseDtos = new List<CourseResponseDto>();

            foreach (var course in courses.Models)
            {
                var instructor = await _supabase.From<Profile>()
                    .Where(p => p.Id == course.InstructorId)
                    .Single();

                var lessonCount = await _supabase.From<Lesson>()
                    .Where(l => l.CourseId == course.Id)
                    .Get();

                var isEnrolled = false;
                var progress = 0;

                if (userId.HasValue)
                {
                    var enrollment = await _supabase.From<Enrollment>()
                        .Where(e => e.StudentId == userId.Value)
                        .Where(e => e.CourseId == course.Id)
                        .Get();

                    if (enrollment.Models.Any())
                    {
                        isEnrolled = true;
                        progress = enrollment.Models.First().Progress;
                    }
                }

                courseDtos.Add(new CourseResponseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    InstructorId = course.InstructorId,
                    InstructorName = instructor.FullName,
                    ThumbnailUrl = course.ThumbnailUrl,
                    Level = course.Level,
                    Category = course.Category,
                    Published = course.Published,
                    LessonCount = lessonCount.Models.Count,
                    IsEnrolled = isEnrolled,
                    Progress = progress,
                    CreatedAt = course.CreatedAt
                });
            }

            return courseDtos;
        }

        public async Task<CourseResponseDto> GetCourseByIdAsync(Guid courseId, Guid? userId = null)
        {
            var course = await _supabase.From<Course>()
                .Where(c => c.Id == courseId)
                .Single();

            var instructor = await _supabase.From<Profile>()
                .Where(p => p.Id == course.InstructorId)
                .Single();

            var lessonCount = await _supabase.From<Lesson>()
                .Where(l => l.CourseId == course.Id)
                .Get();

            var isEnrolled = false;
            var progress = 0;

            if (userId.HasValue)
            {
                var enrollment = await _supabase.From<Enrollment>()
                    .Where(e => e.StudentId == userId.Value)
                    .Where(e => e.CourseId == courseId)
                    .Get();

                if (enrollment.Models.Any())
                {
                    isEnrolled = true;
                    progress = enrollment.Models.First().Progress;
                }
            }

            return new CourseResponseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                InstructorId = course.InstructorId,
                InstructorName = instructor.FullName,
                ThumbnailUrl = course.ThumbnailUrl,
                Level = course.Level,
                Category = course.Category,
                Published = course.Published,
                LessonCount = lessonCount.Models.Count,
                IsEnrolled = isEnrolled,
                Progress = progress,
                CreatedAt = course.CreatedAt
            };
        }

        public async Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto dto, Guid instructorId)
        {
            var course = new Course
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                InstructorId = instructorId,
                ThumbnailUrl = dto.ThumbnailUrl,
                Level = dto.Level,
                Category = dto.Category,
                Published = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _supabase.From<Course>().Insert(course);
            var createdCourse = result.Models.First();

            return await GetCourseByIdAsync(createdCourse.Id);
        }

        public async Task<CourseResponseDto> UpdateCourseAsync(Guid courseId, UpdateCourseDto dto)
        {
            var course = await _supabase.From<Course>()
                .Where(c => c.Id == courseId)
                .Single();

            if (dto.Title != null) course.Title = dto.Title;
            if (dto.Description != null) course.Description = dto.Description;
            if (dto.ThumbnailUrl != null) course.ThumbnailUrl = dto.ThumbnailUrl;
            if (dto.Level != null) course.Level = dto.Level;
            if (dto.Category != null) course.Category = dto.Category;
            if (dto.Published.HasValue) course.Published = dto.Published.Value;

            course.UpdatedAt = DateTime.UtcNow;

            await _supabase.From<Course>().Update(course);

            return await GetCourseByIdAsync(courseId);
        }

        public async Task<bool> DeleteCourseAsync(Guid courseId)
        {
            await _supabase.From<Course>()
                .Where(c => c.Id == courseId)
                .Delete();

            return true;
        }

        public async Task<IEnumerable<LessonResponseDto>> GetCourseLessonsAsync(Guid courseId, Guid? userId = null)
        {
            var lessons = await _supabase.From<Lesson>()
                .Where(l => l.CourseId == courseId)
                .Order("order_index", Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();

            var lessonDtos = new List<LessonResponseDto>();
            Guid? enrollmentId = null;

            if (userId.HasValue)
            {
                var enrollment = await _supabase.From<Enrollment>()
                    .Where(e => e.StudentId == userId.Value)
                    .Where(e => e.CourseId == courseId)
                    .Get();

                enrollmentId = enrollment.Models.FirstOrDefault()?.Id;
            }

            foreach (var lesson in lessons.Models)
            {
                var isCompleted = false;

                if (enrollmentId.HasValue)
                {
                    var progress = await _supabase.From<LessonProgress>()
                        .Where(p => p.EnrollmentId == enrollmentId.Value)
                        .Where(p => p.LessonId == lesson.Id)
                        .Where(p => p.Completed == true)
                        .Get();

                    isCompleted = progress.Models.Any();
                }

                lessonDtos.Add(new LessonResponseDto
                {
                    Id = lesson.Id,
                    CourseId = lesson.CourseId,
                    Title = lesson.Title,
                    Content = lesson.Content,
                    VideoUrl = lesson.VideoUrl,
                    OrderIndex = lesson.OrderIndex,
                    DurationMinutes = lesson.DurationMinutes,
                    IsCompleted = isCompleted,
                    CreatedAt = lesson.CreatedAt
                });
            }

            return lessonDtos;
        }

        public async Task<LessonResponseDto> CreateLessonAsync(Guid courseId, CreateLessonDto dto)
        {
            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                CourseId = courseId,
                Title = dto.Title,
                Content = dto.Content,
                VideoUrl = dto.VideoUrl,
                OrderIndex = dto.OrderIndex,
                DurationMinutes = dto.DurationMinutes,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _supabase.From<Lesson>().Insert(lesson);
            var createdLesson = result.Models.First();

            return new LessonResponseDto
            {
                Id = createdLesson.Id,
                CourseId = createdLesson.CourseId,
                Title = createdLesson.Title,
                Content = createdLesson.Content,
                VideoUrl = createdLesson.VideoUrl,
                OrderIndex = createdLesson.OrderIndex,
                DurationMinutes = createdLesson.DurationMinutes,
                IsCompleted = false,
                CreatedAt = createdLesson.CreatedAt
            };
        }
    }
}
