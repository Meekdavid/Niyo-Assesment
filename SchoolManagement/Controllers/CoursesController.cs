using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManagement.Helpers.DTOs;
using SchoolManagement.Helpers.Models;
using SchoolManagement.Interfaces;

namespace SchoolManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CoursesController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthenticateUser _tokenService;

        public CoursesController(ICourseRepository courseRepository, ILogger<CoursesController> logger, IMapper mapper, IAuthenticateUser tokenService)
        {
            _courseRepository = courseRepository;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
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
                //First, authenticate the JWT token supplied by the client
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Token not found in the request header.");
                    return BadRequest(new BaseResponse { ResponseCode = "01", ResponseMessage = "Token not found" });
                }

                var principal = _tokenService.ValidateToken(token);

                if (principal == null)
                {
                    _logger.LogWarning("Invalid token.");
                    return BadRequest(new BaseResponse { ResponseCode = "02", ResponseMessage = "Invalid token" });
                }

                //Jwt token is valid, proceed to complete the request
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
                //First, authenticate the JWT token supplied by the client
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Token not found in the request header.");
                    return BadRequest(new BaseResponse { ResponseCode = "01", ResponseMessage = "Token not found" });
                }

                var principal = _tokenService.ValidateToken(token);

                if (principal == null)
                {
                    _logger.LogWarning("Invalid token.");
                    return BadRequest(new BaseResponse { ResponseCode = "02", ResponseMessage = "Invalid token" });
                }

                //Jwt token is valid, proceed to complete the request
                var courses = await _courseRepository.GetAllAsync();
                _logger.LogInformation("Courses retrieved successfully.");
                return StatusCode(StatusCodes.Status200OK, courses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when retrieving courses", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }

        // Endpoint to retrieve a course by ID
        [ProducesResponseType(typeof(RetrieveCourseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("{courseId}")]
        public async Task<ActionResult<Course>> GetCourse(string courseId)
        {
            _logger.LogInformation($"Request received to get course with ID: {courseId}");

            try
            {
                //First, authenticate the JWT token supplied by the client
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Token not found in the request header.");
                    return BadRequest(new BaseResponse { ResponseCode = "01", ResponseMessage = "Token not found" });
                }

                var principal = _tokenService.ValidateToken(token);

                if (principal == null)
                {
                    _logger.LogWarning("Invalid token.");
                    return BadRequest(new BaseResponse { ResponseCode = "02", ResponseMessage = "Invalid token" });
                }

                //Jwt token is valid, proceed to complete the request
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
                //First, authenticate the JWT token supplied by the client
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Token not found in the request header.");
                    return BadRequest(new BaseResponse { ResponseCode = "01", ResponseMessage = "Token not found" });
                }

                var principal = _tokenService.ValidateToken(token);

                if (principal == null)
                {
                    _logger.LogWarning("Invalid token.");
                    return BadRequest(new BaseResponse { ResponseCode = "02", ResponseMessage = "Invalid token" });
                }

                //Jwt token is valid, proceed to complete the request
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
        public async Task<IActionResult> DeleteCourse(string courseId)
        {
            _logger.LogInformation($"Request received to delete course with ID: {courseId}");

            try
            {
                //First, authenticate the JWT token supplied by the client
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Token not found in the request header.");
                    return BadRequest(new BaseResponse { ResponseCode = "01", ResponseMessage = "Token not found" });
                }

                var principal = _tokenService.ValidateToken(token);

                if (principal == null)
                {
                    _logger.LogWarning("Invalid token.");
                    return BadRequest(new BaseResponse { ResponseCode = "02", ResponseMessage = "Invalid token" });
                }

                //Jwt token is valid, proceed to complete the request
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
