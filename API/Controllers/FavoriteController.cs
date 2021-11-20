using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers.Common;
using API.Helpers;
using AutoMapper;
using Common.Services;
using DAL.Entities.StudentFavoriteCourses;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using Model.Helper;
using Model.StudentFavoriteCourse.Outputs;
using Services;
using Services.Services;

namespace API.Controllers
{
    public class FavoriteController:BaseController
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _iaccountService;
        private readonly IFavoriteCoursesService _iFavoriteCoursesService;
        public FavoriteController(IMapper mapper, IFavoriteCoursesService IFavoriteCoursesService, IAccountService iaccountService)
        {
            _mapper = mapper;
            _iaccountService = iaccountService;
            _iFavoriteCoursesService = IFavoriteCoursesService;
        }
        [Authorize]
        [HttpPost("List")]
        public async Task<ActionResult<List<FavoriteOutput>>> GetFavoriteList(Paging Params)
        {
            var user = await _iaccountService.GetUserByUserClaim(HttpContext.User);
            var FavoriteList = await _iFavoriteCoursesService.GetFavoriteListAsync(user, Params);
            Response.AddPagination(FavoriteList.CurrentPage, FavoriteList.ItemsPerPage, FavoriteList.TotalItems, FavoriteList.TotalPages);
            return _mapper.Map<List<StudentFavoriteCourse>, List<FavoriteOutput>>(FavoriteList);
        }
        [Authorize]
        [HttpPost("AddTolist")]
        public async Task<ActionResult<ResultService<bool>>> AddTolist(int CourseId) =>
            GetResult<bool>(await _iFavoriteCoursesService.AddToFavoriteAsync(CourseId, await _iaccountService.GetUserByUserClaim(HttpContext.User)));
        [Authorize]
        [HttpPost("RemoveFromList")]
        public async Task<ActionResult<ResultService<bool>>> RemoveFromList(int CourseId) =>
            GetResult<bool>(await _iFavoriteCoursesService.RemoveFromFavoriteAsync(CourseId, await _iaccountService.GetUserByUserClaim(HttpContext.User)));
    }
}