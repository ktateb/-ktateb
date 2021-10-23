using System.Threading.Tasks;
using DAL.Entities.Ratings;
using DAL.Repositories;

namespace Services
{
    public class RatingService : IRatingService
    {
        private readonly IGenericRepository<Rating> _ratingRepository;

        public RatingService(IGenericRepository<Rating> ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public Task<Rating> GetRatingForCourseAsync()
        {
            throw new System.NotImplementedException();
        }   

        public async Task<bool> RatingCourseAsync(Rating rating) =>
            await _ratingRepository.CreateAsync(rating);
    }
    public interface IRatingService
    {
        public Task<bool> RatingCourseAsync(Rating rating);
        public Task<Rating> GetRatingForCourseAsync();
    }
}