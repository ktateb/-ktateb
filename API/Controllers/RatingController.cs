using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using DAL.Entities.Ratings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Rating.Inputs;
using Model.Rating.Outputs;
using Services;

namespace API.Controllers
{
    public class RatingController : BaseController
    {
        private readonly IRatingService _ratingService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public RatingController(IRatingService ratingService, IAccountService accountService, IMapper mapper)
        {
            _ratingService = ratingService;
            _accountService = accountService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Student")]
        [HttpPost("RatingCourse")]
        public async Task<ActionResult> RatingCourse(RatingInput input)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null)
                return Unauthorized("User is Unauthorized, just students can rating course");
            var rating = _mapper.Map<RatingInput, Rating>(input);
            rating.UserId = user.Id;
            await _ratingService.RatingCourseAsync(rating);
            return Ok("Done");
        }

        // [HttpGet("RatingCourse/{id}")]
        // public async Task<RatingOutput> GetRatingCourse(int id)
        // {
        //     // await _accountService.            
        // }

    }
}