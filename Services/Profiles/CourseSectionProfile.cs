using AutoMapper;
using DAL.Entities.Courses;
using Model.CourseSection.Inputs;
using Model.CourseSection.Outputs;

namespace Services.Profiles
{
    public class CourseSectionProfile : Profile
    {
        public CourseSectionProfile()
        {
            CreateMap<CourseSectionCreateInput, CourseSection>()
                .ForMember(dest => dest.SectionTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId));

            CreateMap<CourseSectionUpdateInput, CourseSection>() 
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SectionId))
                .ForMember(dest => dest.SectionTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.Description));

            CreateMap<CourseSection, CourseSectionOutput>() 
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.SectionTitle)); 

        }
    }
}