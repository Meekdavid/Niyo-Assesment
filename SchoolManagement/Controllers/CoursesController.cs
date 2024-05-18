using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManagement.Helpers.DTOs;
using SchoolManagement.Helpers.Models;
using SchoolManagement.Interfaces;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CoursesController> _logger;
        private readonly IMapper _mapper;

        public CoursesController(ICourseRepository courseRepository, ILogger<CoursesController> logger, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _logger = logger;
            _mapper = mapper;
        }

        // Endpoint to create a new course
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost("Create")]
        public async Task<ActionResult<Course>> CreateCourse([FromBody] Course course)
        {
            _logger.LogInformation($"Request received to create course: {JsonConvert.SerializeObject(course)}");

            try
            {
                var successfulCreate = await _courseRepository.AddAsync(course);

                if (successfulCreate)
                {
                    _logger.LogInformation("Course created successfully.");
                    return StatusCode(StatusCodes.Status200OK, new BaseResponse { ResponseCode = "00", ResponseMessage = "Course created successfully" });
                }
                else
                {
                    _logger.LogWarning("Failed to create course.");
                    return StatusCode(StatusCodes.Status200OK ,new BaseResponse { ResponseCode = "01", ResponseMessage = "Unable to create course" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when creating a course: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }

        // Endpoint to retrieve all courses
        [ProducesResponseType(typeof(IEnumerable<Course>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("RetrieveAll")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            _logger.LogInformation("Request received to get all courses.");

            try
            {
                var courses = await _courseRepository.GetAllAsync();
                _logger.LogInformation("Courses retrieved successfully.");
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when retrieving courses: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new NotSuccessfulResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }

        // Endpoint to retrieve a course by ID
        [ProducesResponseType(typeof(RetrieveCourseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("{courseId}")]
        public async Task<ActionResult<Course>> GetCourse(int courseId)
        {
            _logger.LogInformation($"Request received to get course with ID: {courseId}");

            try
            {
                var course = await _courseRepository.GetByIdAsync(courseId);
                if (course == null)
                {
                    _logger.LogWarning($"Course with ID: {courseId} not found.");
                    return NotFound(new BaseResponse { ResponseCode = "01", ResponseMessage = "Course not found" });
                }
                _logger.LogInformation($"Course with ID: {courseId} retrieved successfully.");
                return StatusCode(StatusCodes.Status200OK ,new RetrieveCourseResponse { Courses = (IEnumerable<Course>)course, ResponseCode = "00", ResponseMessage = "Retreived Successfully"});
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when retrieving course with ID: {courseId}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }

        // Endpoint to update a course by ID
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateCourse(int courseId, [FromBody] CourseDTO course)
        {
            _logger.LogInformation($"Request received to update course with ID: {courseId}");

            try
            {
                var courseModel = _mapper.Map<Course>(course);
                var succesfulUpdate = await _courseRepository.UpdateAsync(courseModel);

                if (succesfulUpdate)
                {
                    _logger.LogInformation($"Course with ID: {courseId} updated successfully.");
                    return StatusCode(StatusCodes.Status200OK, new BaseResponse { ResponseCode = "00", ResponseMessage = "Unable to update course" });
                }
                else
                {
                    _logger.LogWarning($"Failed to update course with ID: {courseId}.");
                    return StatusCode(StatusCodes.Status200OK ,new BaseResponse { ResponseCode = "01", ResponseMessage = "Unable to update course" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when updating course with ID: {courseId}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }

        // Endpoint to delete a course by ID
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            _logger.LogInformation($"Request received to delete course with ID: {courseId}");

            try
            {
                var successfulDelete = await _courseRepository.DeleteAsync(courseId);

                if (successfulDelete)
                {
                    _logger.LogInformation($"Course with ID: {courseId} deleted successfully.");
                    return StatusCode(StatusCodes.Status200OK ,new BaseResponse { ResponseCode = "00", ResponseMessage = "Course Deleted Sucessfully" });
                }
                else
                {
                    _logger.LogWarning($"Failed to delete course with ID: {courseId}.");
                    return StatusCode(StatusCodes.Status200OK ,new BaseResponse { ResponseCode = "01", ResponseMessage = "Unable to delete course" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when deleting course with ID: {courseId}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }
    }

}
