using System.Linq;
using AutoMapper;
using DAL.Entities.Identity;
using Model.User.Inputs;
using Model.User.Outputs;

namespace Services.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UsersOutput>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Name)));

            CreateMap<User, UserOutput>()
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => "Token not implemment yet!"))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Name)));
            
            CreateMap<UserUpdateInput , User>();
        }
    }
}