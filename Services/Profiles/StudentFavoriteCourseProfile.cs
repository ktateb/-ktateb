using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Entities.StudentFavoriteCourses;
using Model.StudentFavoriteCourse.Outputs;

namespace Services.Profiles
{
    public class StudentFavoriteCourseProfile : Profile
    {
        public StudentFavoriteCourseProfile()
        {
            CreateMap<StudentFavoriteCourse, FavoriteOutput>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.DateAdded, opt => opt.MapFrom(src => src.AddedDate));

        }
    }
}