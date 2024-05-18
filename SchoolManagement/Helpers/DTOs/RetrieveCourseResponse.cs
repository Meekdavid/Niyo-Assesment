using SchoolManagement.Helpers.Models;

namespace SchoolManagement.Helpers.DTOs
{
    public class RetrieveCourseResponse : BaseResponse
    {
        public IEnumerable<Course> Courses { get; set; }
    }
}
