using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduPlatform.API.DTOs;
using EduPlatform.API.Services;
using System.Security.Claims;

namespace EduPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetAllCourses()
        {
            try
            {
                var userId = GetCurrentUserId();
                var courses = await _courseService.GetAllCoursesAsync(userId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseResponseDto>> GetCourse(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var course = await _courseService.GetCourseByIdAsync(id, userId);
                return Ok(course);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CourseResponseDto>> CreateCourse([FromBody] CreateCourseDto dto)
        {
            try
            {
                var userId = GetCurrentUserId() ?? throw new Exception("User not authenticated");
                var course = await _courseService.CreateCourseAsync(dto, userId.Value);
                return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<CourseResponseDto>> UpdateCourse(Guid id, [FromBody] UpdateCourseDto dto)
        {
            try
            {
                var course = await _courseService.UpdateCourseAsync(id, dto);
                return Ok(course);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(Guid id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/lessons")]
        public async Task<ActionResult<IEnumerable<LessonResponseDto>>> GetCourseLessons(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var lessons = await _courseService.GetCourseLessonsAsync(id, userId);
                return Ok(lessons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("{id}/lessons")]
        public async Task<ActionResult<LessonResponseDto>> CreateLesson(Guid id, [FromBody] CreateLessonDto dto)
        {
            try
            {
                var lesson = await _courseService.CreateLessonAsync(id, dto);
                return CreatedAtAction(nameof(GetCourseLessons), new { id }, lesson);
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
}
