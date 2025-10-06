using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace EduPlatform.API.Models
{
    [Table("lessons")]
    public class Lesson : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("course_id")]
        public Guid CourseId { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("video_url")]
        public string? VideoUrl { get; set; }

        [Column("order_index")]
        public int OrderIndex { get; set; }

        [Column("duration_minutes")]
        public int? DurationMinutes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
