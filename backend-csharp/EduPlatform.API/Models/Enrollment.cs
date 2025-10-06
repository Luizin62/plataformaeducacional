using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace EduPlatform.API.Models
{
    [Table("enrollments")]
    public class Enrollment : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("course_id")]
        public Guid CourseId { get; set; }

        [Column("enrolled_at")]
        public DateTime EnrolledAt { get; set; }

        [Column("completed")]
        public bool Completed { get; set; }

        [Column("progress")]
        public int Progress { get; set; }
    }

    [Table("lesson_progress")]
    public class LessonProgress : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("enrollment_id")]
        public Guid EnrollmentId { get; set; }

        [Column("lesson_id")]
        public Guid LessonId { get; set; }

        [Column("completed")]
        public bool Completed { get; set; }

        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }
    }
}
