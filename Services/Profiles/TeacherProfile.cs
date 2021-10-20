using AutoMapper;
using DAL.Entities.Teachers;
using Model.Teacher.Inputs;
using Model.Teacher.Outputs;

namespace Services.Profiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<TeacherUpdateInput, Teacher>() 
                .ForMember(dest => dest.AboutMe, opt => opt.MapFrom(src => src.AboutMe)) 
                .ForMember(dest => dest.FaceBookUrl, opt => opt.MapFrom(src => src.FaceBookUrl))  
                .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))  
                .ForMember(dest => dest.Specialist, opt => opt.MapFrom(src => src.Specialist))  
                .ForMember(dest => dest.TelegramUrl, opt => opt.MapFrom(src => src.TelegramUrl)) 
                .ForMember(dest => dest.WhatsappUrl, opt => opt.MapFrom(src => src.WhatsappUrl));
            CreateMap<Teacher, TeacherOutput>() 
                .ForMember(dest => dest.AboutMe, opt => opt.MapFrom(src => src.AboutMe)) 
                .ForMember(dest => dest.FaceBookUrl, opt => opt.MapFrom(src => src.FaceBookUrl))  
                .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))  
                .ForMember(dest => dest.Specialist, opt => opt.MapFrom(src => src.Specialist))  
                .ForMember(dest => dest.TelegramUrl, opt => opt.MapFrom(src => src.TelegramUrl))  
                .ForMember(dest => dest.WhatsappUrl, opt => opt.MapFrom(src => src.WhatsappUrl));
        }
    }
}