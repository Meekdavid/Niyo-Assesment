namespace SchoolManagement.Helpers.Models
{
    public class AuthUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string? role { get; set; }
        public int phoneNumber { get; set; }
        public int locationId { get; set; }
        public int typeId { get; set; }
    }
}
