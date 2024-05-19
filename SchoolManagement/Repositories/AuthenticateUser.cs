using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Helpers.DTOs;
using SchoolManagement.Helpers.Models;
using SchoolManagement.Interfaces;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagement.Repositories
{
    public class AuthenticateUser : IAuthenticateUser
    {
        private readonly IAuthUserRepository _authUserRepository;
        private readonly IConfiguration _configuration;

        public AuthenticateUser(IAuthUserRepository authUserRepository, IConfiguration configuration)
        {
            _authUserRepository = authUserRepository;
            _configuration = configuration;
        }

        public async Task<AuthenticateUserModel> Authenticate(LoginDTO login)
        {
            //First Check if user exists on the Database
            var currentUser = (await _authUserRepository.GetByIdAsync(login.userName));

            if (currentUser == null)
            {
                //Return the appriprate response is user does not exist
                return new AuthenticateUserModel
                {
                    userInfo = null,
                    ResponseMessage = "User Not Found"
                };

            }

            if (currentUser.userName == login.userName && BCrypt.Net.BCrypt.Verify(login.password, currentUser.password))
            {
                //Return the appriprate response is authenticated
                return new AuthenticateUserModel
                {
                    userInfo = currentUser,
                    ResponseMessage = "User Authenticated"
                };
            }
            else
            {
                //Return the appriprate response is wrong user credentials were provided
                return new AuthenticateUserModel
                {
                    userInfo = null,
                    ResponseMessage = "Incorrect Username or Password"
                };
            }
        }


        public async Task<string> GenerateJwtToken(AuthUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.userName),
                new Claim(ClaimTypes.Email, user.userName),
                new Claim(ClaimTypes.GivenName, user.firstName),
                new Claim(ClaimTypes.Surname, user.lastName),
                new Claim(ClaimTypes.Role, user.role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<string> HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<ClaimsPrincipal> ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                Log.Error("Token validation failed", ex);
                return null;
            }
        }
    }
}
