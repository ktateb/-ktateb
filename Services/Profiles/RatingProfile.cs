using AutoMapper;
using DAL.Entities.Ratings;
using Model.Rating.Inputs;

namespace Services.Profiles
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<RatingInput, Rating>();
        }
    }
}