using SchoolManagement.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Helpers.DTOs
{
    public class AuthUserRequest
    {
        [Validate]
        [Required]
        public string firstName { get; set; }
        [Validate]
        [Required]
        public string lastName { get; set; }
        [Validate]
        [Required]
        [EmailAddress]
        public string userName { get; set; }
        [Validate]
        [Required]
        public string password { get; set; }
        [Validate]
        [Required]
        public int phoneNumber { get; set; }
        [Validate]
        public int locationId { get; set; }
        [Validate]
        public int typeId { get; set; }
    }
}
