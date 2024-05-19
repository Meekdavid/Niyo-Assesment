using Microsoft.AspNetCore.SignalR;
using SchoolManagement.Helpers.DBContext;
using SchoolManagement.Helpers.Models;
using SchoolManagement.Helpers.SignalR;
using SchoolManagement.Interfaces;

namespace SchoolManagement.Repositories
{
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(SchoolDbContext context, IHubContext<SchoolHub> hubContext) : base(context, hubContext)
        {
        }

        
    }

}
