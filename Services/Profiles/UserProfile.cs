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

            CreateMap<User, UserSeedOutput>();
            CreateMap<User, UsernameAndRolesOnly>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Name).ToList()));

            CreateMap<User, UsersOutput>()
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => "https://localhost:5001/" + src.PictureUrl))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Name).ToList()));

            CreateMap<User, UserOutput>()
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => "https://localhost:5001/" + src.PictureUrl))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Birhtday, opt => opt.MapFrom(src => src.Birthday))
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Name).ToList()));

            CreateMap<UserUpdate, User>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender));
        }
    }
}