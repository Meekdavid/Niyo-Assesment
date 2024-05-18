using SchoolManagement.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Helpers.DTOs
{
    public class StudentDTO
    {
        [Validate]
        public string FirstName { get; set; }
        [Validate]
        public string LastName { get; set; }
        [Validate]
        public string DateOfBirth { get; set; }
        [Validate]
        [EmailAddress]
        public string Email { get; set; }
        [Validate]
        public string PhoneNumber { get; set; }
        [Validate]
        public string Address { get; set; }
        [Validate]
        public string EnrollmentDate { get; set; }
        [Validate]
        public double GPA { get; set; }
    }
}
