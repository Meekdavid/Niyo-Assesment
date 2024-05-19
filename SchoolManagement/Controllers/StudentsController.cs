using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManagement.Helpers.DTOs;
using SchoolManagement.Helpers.Models;
using SchoolManagement.Interfaces;

namespace SchoolManagement.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<StudentsController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthenticateUser _tokenService;

        public StudentsController(IStudentRepository studentRepository, ILogger<StudentsController> logger, IMapper mapper, IAuthenticateUser tokenService)
        {
            _studentRepository = studentRepository;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        // Endpoint to create a new student
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost("CreateStudent")]
        public async Task<ActionResult<Student>> CreateStudent([FromBody] StudentDTO student)
        {
            _logger.LogInformation($"Request received to create student: {JsonConvert.SerializeObject(student)}");

            try
            {
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

                var ran = new Random();

                var studentModel = _mapper.Map<Student>(student);
                studentModel.Id = student.Email;
                var successful = await _studentRepository.AddAsync(studentModel);

                if (successful)
                {
                    _logger.LogInformation("Student created successfully.");
                    return StatusCode(StatusCodes.Status201Created,new BaseResponse {ResponseCode = "00", ResponseMessage = "Student Created Sucessfully" });
                }
                else
                {
                    _logger.LogWarning("Failed to create student.");
                    return BadRequest(new BaseResponse { ResponseCode = "01", ResponseMessage = "Unable to create student" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when creating a student: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }

        // Endpoint to retrieve all students
        [ProducesResponseType(typeof(IEnumerable<CreateStudentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            _logger.LogInformation("Request received to get all students.");

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
                var students = await _studentRepository.GetAllAsync();
                _logger.LogInformation("Students retrieved successfully.");
                return StatusCode(StatusCodes.Status200OK, new CreateStudentResponse { StudentInfo = students, ResponseCode = "00", ResponseMessage = "Successful Fetch" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when retrieving students: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }

        // Endpoint to retrieve a student by ID
        [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("{studentId}")]
        public async Task<ActionResult<Student>> GetStudent(string studentId)
        {
            _logger.LogInformation($"Request received to get student with ID: {studentId}");

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
                var student = await _studentRepository.GetByIdAsync(studentId);
                if (student == null)
                {
                    _logger.LogWarning($"Student with ID: {studentId} not found.");
                    return NotFound(new BaseResponse { ResponseCode = "01", ResponseMessage = "Student not found" });
                }
                _logger.LogInformation($"Student with ID: {studentId} retrieved successfully.");
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when retrieving student with ID: {studentId} - {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }

        // Endpoint to update a student by ID
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut("{studentId}")]
        public async Task<IActionResult> UpdateStudent(int studentId, [FromBody] StudentDTO student)
        {
            _logger.LogInformation($"Request received to update student with ID: {studentId}");

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
                var studentModel = _mapper.Map<Student>(student);

                var successful = await _studentRepository.UpdateAsync(studentModel);

                if (successful)
                {
                    _logger.LogInformation($"Student with ID: {studentId} updated successfully.");
                    return StatusCode(StatusCodes.Status200OK, new BaseResponse { ResponseCode = "00", ResponseMessage = "Student Details Updated Sucessfully" }); ;
                }
                else
                {
                    _logger.LogWarning($"Failed to update student with ID: {studentId}.");
                    return StatusCode(StatusCodes.Status200OK ,new BaseResponse { ResponseCode = "01", ResponseMessage = "Unable to update student" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when updating student with ID: {studentId} - {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }

        // Endpoint to delete a student by ID
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(string studentId)
        {
            _logger.LogInformation($"Request received to delete student with ID: {studentId}");

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
                var successful = await _studentRepository.DeleteAsync(studentId);

                if (successful)
                {
                    _logger.LogInformation($"Student with ID: {studentId} deleted successfully.");
                    return StatusCode(StatusCodes.Status200OK, new BaseResponse { ResponseCode = "00", ResponseMessage = "Student Deleted Sucessfully" });
                }
                else
                {
                    _logger.LogWarning($"Failed to delete student with ID: {studentId}.");
                    return StatusCode (StatusCodes.Status200OK ,new BaseResponse { ResponseCode = "01", ResponseMessage = "Unable to delete student" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when deleting student with ID: {studentId} - {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }
    }

}
