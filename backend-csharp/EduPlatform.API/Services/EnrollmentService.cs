using EduPlatform.API.Models;
using Supabase;

namespace EduPlatform.API.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly Client _supabase;

        public EnrollmentService(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<Enrollment> EnrollInCourseAsync(Guid studentId, Guid courseId)
        {
            var existingEnrollment = await _supabase.From<Enrollment>()
                .Where(e => e.StudentId == studentId)
                .Where(e => e.CourseId == courseId)
                .Get();

            if (existingEnrollment.Models.Any())
            {
                return existingEnrollment.Models.First();
            }

            var enrollment = new Enrollment
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                CourseId = courseId,
                EnrolledAt = DateTime.UtcNow,
                Completed = false,
                Progress = 0
            };

            var result = await _supabase.From<Enrollment>().Insert(enrollment);
            return result.Models.First();
        }

        public async Task<IEnumerable<Enrollment>> GetStudentEnrollmentsAsync(Guid studentId)
        {
            var enrollments = await _supabase.From<Enrollment>()
                .Where(e => e.StudentId == studentId)
                .Order("enrolled_at", Supabase.Postgrest.Constants.Ordering.Descending)
                .Get();

            return enrollments.Models;
        }

        public async Task<bool> UpdateLessonProgressAsync(Guid enrollmentId, Guid lessonId, bool completed)
        {
            var existingProgress = await _supabase.From<LessonProgress>()
                .Where(p => p.EnrollmentId == enrollmentId)
                .Where(p => p.LessonId == lessonId)
                .Get();

            if (completed)
            {
                if (!existingProgress.Models.Any())
                {
                    var progress = new LessonProgress
                    {
                        Id = Guid.NewGuid(),
                        EnrollmentId = enrollmentId,
                        LessonId = lessonId,
                        Completed = true,
                        CompletedAt = DateTime.UtcNow
                    };

                    await _supabase.From<LessonProgress>().Insert(progress);
                }
            }
            else
            {
                if (existingProgress.Models.Any())
                {
                    await _supabase.From<LessonProgress>()
                        .Where(p => p.EnrollmentId == enrollmentId)
                        .Where(p => p.LessonId == lessonId)
                        .Delete();
                }
            }

            var enrollment = await _supabase.From<Enrollment>()
                .Where(e => e.Id == enrollmentId)
                .Single();

            var allProgress = await _supabase.From<LessonProgress>()
                .Where(p => p.EnrollmentId == enrollmentId)
                .Where(p => p.Completed == true)
                .Get();

            var course = await _supabase.From<Course>()
                .Where(c => c.Id == enrollment.CourseId)
                .Single();

            var totalLessons = await _supabase.From<Lesson>()
                .Where(l => l.CourseId == course.Id)
                .Get();

            if (totalLessons.Models.Count > 0)
            {
                var newProgress = (int)Math.Round((double)allProgress.Models.Count / totalLessons.Models.Count * 100);
                enrollment.Progress = newProgress;
                enrollment.Completed = newProgress == 100;

                await _supabase.From<Enrollment>().Update(enrollment);
            }

            return true;
        }
    }
}
