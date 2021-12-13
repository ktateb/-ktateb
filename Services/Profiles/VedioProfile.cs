using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Entities.Courses;
using Model.Vedio;

namespace Services.Profiles
{
    public class VedioProfile : Profile
    {
        public VedioProfile()
        {
            CreateMap<VedioInput, CourseVedio>() 
                .ForMember(dest => dest.SectionId, opt => opt.MapFrom(src => src.SectionId))
                .ForMember(dest => dest.VedioTitle, opt => opt.MapFrom(src => src.VedioTitle))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription));
            CreateMap<VedioUpdateInput, CourseVedio>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.VedioTitle, opt => opt.MapFrom(src => src.VedioTitle))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription));
            CreateMap<CourseVedio, VedioOutput>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SectionId, opt => opt.MapFrom(src => src.SectionId))
                .ForMember(dest => dest.VedioTitle, opt => opt.MapFrom(src => src.VedioTitle))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(dest => dest.AddedDate, opt => opt.MapFrom(src => src.AddedDate))
                .ForMember(dest => dest.ImgeURL, opt => opt.MapFrom(src => src.ImgeURL))
                .ForMember(dest => dest.TimeInSeconds, opt => opt.MapFrom(src => src.TimeInSeconds))
                .ForMember(dest => dest.VedioURL, opt => opt.MapFrom(src => src.URL));

            CreateMap<CourseVedio, VedioListOutput>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SectionId, opt => opt.MapFrom(src => src.SectionId))
                .ForMember(dest => dest.VedioTitle, opt => opt.MapFrom(src => src.VedioTitle))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(dest => dest.AddedDate, opt => opt.MapFrom(src => src.AddedDate))
                .ForMember(dest => dest.ImgeURL, opt => opt.MapFrom(src => src.ImgeURL))
                .ForMember(dest => dest.TimeInSeconds, opt => opt.MapFrom(src => src.TimeInSeconds));
        }
    }
}