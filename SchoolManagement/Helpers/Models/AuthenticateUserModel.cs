using SchoolManagement.Helpers.DTOs;

namespace SchoolManagement.Helpers.Models
{
    public class AuthenticateUserModel : BaseResponse
    {
        public AuthUser userInfo { get; set; }
    }
}
