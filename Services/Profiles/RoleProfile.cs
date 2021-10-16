using AutoMapper;
using DAL.Entities.Identity;
using Model.Role.Inputs;
using Model.Role.Outputs;

namespace Services.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleInput, Role>();
            CreateMap<Role, RoleOutput>();
            CreateMap<RoleUpdate, Role>();

        }
    }
}