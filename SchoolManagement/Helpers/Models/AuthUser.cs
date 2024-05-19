using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Helpers.Models
{
    public class AuthUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        [Key]
        public string userName { get; set; }
        public string password { get; set; }
        public string? role { get; set; }
        public string phoneNumber { get; set; }
        public string locationId { get; set; }
        public string typeId { get; set; }
    }
}
