using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Helpers.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public string userName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
    }
}
