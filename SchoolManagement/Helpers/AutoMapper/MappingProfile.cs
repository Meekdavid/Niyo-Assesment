using AutoMapper;
using SchoolManagement.Helpers.DTOs;
using SchoolManagement.Helpers.Models;

namespace ProgramApi.Helpers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentDTO, Student>().ReverseMap();
            CreateMap<CourseDTO, Course>().ReverseMap();
            CreateMap<AuthUserRequest, AuthUser>().ReverseMap();
            
        }
    }

}
