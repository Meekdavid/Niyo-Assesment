using SchoolManagement.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Helpers.DTOs
{
    public class AuthUserRequest
    {

        [Validate]
        public string firstName { get; set; }
        [Validate]
        public string lastName { get; set; }
        [Validate]
        [Required]
        [EmailAddress]
        public string userName { get; set; }
        [Validate]
        public string password { get; set; }
        [Validate]
        public string phoneNumber { get; set; }
        [Validate]
        public string locationId { get; set; }
        [Validate]
        public string typeId { get; set; }
    }
}
