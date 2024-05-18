using SchoolManagement.Helpers.Models;

namespace SchoolManagement.Helpers.DTOs
{
    public class CreateStudentResponse : BaseResponse
    {
        public IEnumerable<Student> StudentInfo { get; set; }
    }
}
