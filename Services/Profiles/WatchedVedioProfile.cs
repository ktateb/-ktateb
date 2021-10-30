using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Entities.StudentWatches;
using Model.StudentWatchedVedio.Outputs;

namespace Services.Profiles
{
    public class WatchedVedioProfile : Profile
    {
        public WatchedVedioProfile()
        {
            CreateMap<StudentWatchedVedio, WatchedVedioOutput>()
                .ForMember(dest => dest.VedioId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.WatchedDate, opt => opt.MapFrom(src => src.WatchedDate));
        }
    }
}