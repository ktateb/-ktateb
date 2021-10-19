using System.Linq;
using AutoMapper;
using DAL.Entities.Courses;
using DAL.Entities.Teachers;
using Model.Course.Inputs;
using Model.Course.Outputs;
using static Model.Course.Outputs.CourseOutput;

namespace Services.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseCreateInput, Course>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.OverViewDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.LearnListDescription, opt => opt.MapFrom(src => src.LearnListDescription))
                .ForMember(dest => dest.ThisCourseFor, opt => opt.MapFrom(src => src.ThisCourseFor))
                .ForMember(dest => dest.CourseRequerment, opt => opt.MapFrom(src => src.PreRequerment))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<CourseUpdateInput, Course>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.OverViewDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.LearnListDescription, opt => opt.MapFrom(src => src.LearnListDescription))
                .ForMember(dest => dest.ThisCourseFor, opt => opt.MapFrom(src => src.ThisCourseFor))
                .ForMember(dest => dest.CourseRequerment, opt => opt.MapFrom(src => src.PreRequerment))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<Course, CourseOutput>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.OverViewDescription))
                .ForMember(dest => dest.LearnListDescription, opt => opt.MapFrom(src => src.LearnListDescription))
                .ForMember(dest => dest.ThisCourseFor, opt => opt.MapFrom(src => src.ThisCourseFor))
                .ForMember(dest => dest.PreRequerment, opt => opt.MapFrom(src => src.CourseRequerment))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TeacherUserName, opt => opt.MapFrom(src => src.Teacher.User.UserName))
                .ForMember(dest => dest.TeacherDisplayrName, opt => opt.MapFrom(src => src.Teacher.User.FullName));
            CreateMap<Course, TeacherCourseOutput>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Tilte, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.OverViewDescription));


        }
    }
}