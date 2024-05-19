using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManagement.Helpers.DTOs;
using SchoolManagement.Helpers.Models;
using SchoolManagement.Interfaces;
using System;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthUserRepository _authUserRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAuthenticateUser _authenticateUser;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticationController(
            IAuthUserRepository authUserRepository,
            IStudentRepository studentRepository,
            IAuthenticateUser authenticateUser,
            ILogger<AuthenticationController> logger,
            IConfiguration configuration, IMapper mapper)
        {
            _authUserRepository = authUserRepository;
            _studentRepository = studentRepository;
            _authenticateUser = authenticateUser;
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
        }

        // Endpoint to register a new auth user
        [ProducesResponseType(typeof(AuthUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost("Register")]
        public async Task<ActionResult<AuthUser>> Register([FromBody] AuthUserRequest authUser)
        {
            _logger.LogInformation($"Request received to register auth user: {JsonConvert.SerializeObject(authUser)}");

            try
            {
                // First check if the user exists
                var userExists = await _authUserRepository.GetByIdAsync(authUser.userName);

                if(userExists != null)
                {
                    _logger.LogInformation($"Unable to create user for user name {authUser.userName}, because user already exists");
                    return StatusCode(StatusCodes.Status200OK ,new BaseResponse { ResponseCode = "01", ResponseMessage = "User already exists"});
                }

                // Hash the password before saving
                authUser.password = await _authenticateUser.HashPassword(authUser.password);

                // Add auth user to the database
                var authUserModel = _mapper.Map<AuthUser>(authUser);
                authUserModel.role = (authUser.typeId == "1") ? "admin" : ((authUser.typeId == "2") ? "teacher" : "student");//Assign user roles
                var authUserCreated = await _authUserRepository.AddAsync(authUserModel);

                if (authUserCreated)
                {
                    // If user is a student, add the student information
                    if (authUser.typeId == "3")
                    {
                        Random randomNumbers = new Random();
                        var student = new Student
                        {
                            Id = randomNumbers.Next(100000000, 999999999).ToString(),
                            FirstName = authUser.firstName,
                            LastName = authUser.lastName,
                            Email = authUser.userName,
                            EnrollmentDate = DateTime.Now.ToString()
                        };

                        await _studentRepository.AddAsync(student);
                    }

                    _logger.LogInformation("Auth user registered successfully.");
                    return StatusCode(StatusCodes.Status201Created, new BaseResponse { ResponseCode = "00", ResponseMessage = "User created successfully" });
                }
                else
                {
                    _logger.LogWarning("Failed to register auth user.");
                    return StatusCode(StatusCodes.Status200OK ,new BaseResponse { ResponseCode = "01", ResponseMessage = "Unable to register user" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when registering an auth user: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }

        // Endpoint to login an auth user
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginDTO loginDto)
        {
            _logger.LogInformation($"Request received to login auth user: {JsonConvert.SerializeObject(loginDto)}");

            try
            {
                // Authenticate the user
                var authResult = await _authenticateUser.Authenticate(loginDto);

                if (authResult.userInfo != null)
                {
                    // Generate JWT token, if user is authenticated
                    var jwtToken = await _authenticateUser.GenerateJwtToken(authResult.userInfo);

                    _logger.LogInformation("Auth user logged in successfully.");
                    return StatusCode(StatusCodes.Status200OK, new LoginResponse { JwtToken = jwtToken, ResponseCode = "00", ResponseMessage = "Login successful" });
                }
                else
                {
                    _logger.LogWarning($"Authentication failed for user: {loginDto.userName}");
                    return StatusCode(StatusCodes.Status200OK ,new BaseResponse { ResponseCode = "01", ResponseMessage = authResult.ResponseMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred when logging in an auth user: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { ResponseCode = "06", ResponseMessage = "An error occurred, please try again" });
            }
        }
    }
}
