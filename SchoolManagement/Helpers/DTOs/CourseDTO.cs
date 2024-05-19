using SchoolManagement.Helpers.Attributes;

namespace SchoolManagement.Helpers.DTOs
{
    public class CourseDTO
    {
        [Validate]
        public string Title { get; set; }
        [Validate]
        public string Description { get; set; }
        [Validate]
        public string Credits { get; set; }
        [Validate]
        public string Instructor { get; set; }
        [Validate]
        public string Department { get; set; }
        [Validate]
        public string StartDate { get; set; }
        [Validate]
        public string EndDate { get; set; }
    }
}
