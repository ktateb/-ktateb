using AutoMapper;
using DAL.Entities.Countries;
using Model.Country.Outputs;

namespace Services.Profiles
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryOutput>();
        }
    }
}