using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using Common.Services;
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

        public RatingController(IRatingService ratingService, IAccountService accountService)
        {
            _ratingService = ratingService;
            _accountService = accountService;
        }

        // [Authorize(Roles = "Student")]
        [HttpPost("RatingCourse")]
        public async Task<ActionResult<ResultService<bool>>> RatingCourse(RatingInput input) =>
            GetResult(await _ratingService.RatingCourseAsync(input, await _accountService.GetUserByUserClaim(HttpContext.User)));

        [HttpDelete("DeleteRating/{id}")]
        public async Task<ActionResult<ResultService<bool>>> DeleteRating(int id) =>
            GetResult(await _ratingService.DeleteRating(id, await _accountService.GetUserByUserClaim(HttpContext.User)));

        [HttpGet("ratings")]
        public ActionResult<ResultService<Dictionary<string, int>>> GetRatingsData() =>
            GetResult(_ratingService.GetRatingsData());

        [HttpGet("RatingCourse/{id}")]
        public async Task<ActionResult<ResultService<RatingOutput>>> GetRatingCourse(int id) =>
            GetResult(await _ratingService.GetRatingForCourseAsync(id));
    }
}