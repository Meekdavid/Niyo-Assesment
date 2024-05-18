using SchoolManagement.Helpers.DTOs;
using SchoolManagement.Helpers.Models;
using SchoolManagement.Interfaces;

namespace SchoolManagement.Repositories
{
    public class AuthenticateUser : IAuthenticateUser
    {
        public Task<AuthUser> Authenticate(LoginDTO login)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateJwtToken(AuthUser user)
        {
            throw new NotImplementedException();
        }
    }
}
