using SchoolManagement.Helpers.DTOs;
using SchoolManagement.Helpers.Models;
using System.Security.Claims;

namespace SchoolManagement.Interfaces
{
    public interface IAuthenticateUser
    {
        Task<AuthenticateUserModel> Authenticate(LoginDTO login);
        Task<string> GenerateJwtToken(AuthUser user);
        Task<string> HashPassword(string password);
        //Task<ClaimsPrincipal> ValidateToken(string token);
    }
}
