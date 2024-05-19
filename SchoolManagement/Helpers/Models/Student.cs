using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Helpers.Models
{
    public class Student
    {
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string EnrollmentDate { get; set; }
        public string GPA { get; set; }
    }

}
