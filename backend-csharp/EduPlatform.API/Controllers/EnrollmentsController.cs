using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduPlatform.API.Services;
using EduPlatform.API.Models;
using System.Security.Claims;

namespace EduPlatform.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        public async Task<ActionResult<Enrollment>> EnrollInCourse([FromBody] EnrollmentRequest request)
        {
            try
            {
                var userId = GetCurrentUserId() ?? throw new Exception("User not authenticated");
                var enrollment = await _enrollmentService.EnrollInCourseAsync(userId.Value, request.CourseId);
                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetMyEnrollments()
        {
            try
            {
                var userId = GetCurrentUserId() ?? throw new Exception("User not authenticated");
                var enrollments = await _enrollmentService.GetStudentEnrollmentsAsync(userId.Value);
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{enrollmentId}/lessons/{lessonId}")]
        public async Task<ActionResult> UpdateLessonProgress(
            Guid enrollmentId,
            Guid lessonId,
            [FromBody] LessonProgressRequest request)
        {
            try
            {
                await _enrollmentService.UpdateLessonProgressAsync(enrollmentId, lessonId, request.Completed);
                return Ok(new { message = "Progress updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return null;
        }
    }

    public class EnrollmentRequest
    {
        public Guid CourseId { get; set; }
    }

    public class LessonProgressRequest
    {
        public bool Completed { get; set; }
    }
}
