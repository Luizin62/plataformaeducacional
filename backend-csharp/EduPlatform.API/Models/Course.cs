using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace EduPlatform.API.Models
{
    [Table("courses")]
    public class Course : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("instructor_id")]
        public Guid InstructorId { get; set; }

        [Column("thumbnail_url")]
        public string? ThumbnailUrl { get; set; }

        [Column("level")]
        public string Level { get; set; } = "beginner";

        [Column("category")]
        public string Category { get; set; } = string.Empty;

        [Column("published")]
        public bool Published { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public class CourseWithDetails : Course
    {
        public string? InstructorName { get; set; }
        public int LessonCount { get; set; }
        public bool IsEnrolled { get; set; }
        public int Progress { get; set; }
    }
}
