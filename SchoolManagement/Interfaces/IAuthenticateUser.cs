using SchoolManagement.Helpers.DTOs;
using SchoolManagement.Helpers.Models;

namespace SchoolManagement.Interfaces
{
    public interface IAuthenticateUser
    {
        Task<AuthUser> Authenticate(LoginDTO login);
        Task<string> GenerateJwtToken(AuthUser user);
    }
}
