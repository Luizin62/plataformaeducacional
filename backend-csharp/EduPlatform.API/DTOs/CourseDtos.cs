namespace EduPlatform.API.DTOs
{
    public class CreateCourseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public string Level { get; set; } = "beginner";
        public string Category { get; set; } = string.Empty;
    }

    public class UpdateCourseDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Level { get; set; }
        public string? Category { get; set; }
        public bool? Published { get; set; }
    }

    public class CourseResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid InstructorId { get; set; }
        public string? InstructorName { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool Published { get; set; }
        public int LessonCount { get; set; }
        public bool IsEnrolled { get; set; }
        public int Progress { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateLessonDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public int OrderIndex { get; set; }
        public int? DurationMinutes { get; set; }
    }

    public class LessonResponseDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public int OrderIndex { get; set; }
        public int? DurationMinutes { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
