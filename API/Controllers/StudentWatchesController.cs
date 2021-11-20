using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers.Common;
using API.Helpers;
using AutoMapper;
using Common.Services;
using DAL.Entities.StudentWatches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Helper;
using Model.StudentWatchedVedio.Outputs;
using Services;
using Services.Services;

namespace API.Controllers
{
    public class StudentWatchesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _iaccountService;
        private readonly IStudentWatchesService _iStudentWatchesService;
        public StudentWatchesController(IMapper mapper, IStudentWatchesService iStudentWatchesController, IAccountService iaccountService)
        {
            _mapper = mapper;
            _iaccountService = iaccountService;
            _iStudentWatchesService = iStudentWatchesController;
        }

        [Authorize]
        [HttpPost("List")]
        public async Task<ActionResult<List<WatchedVedioOutput>>> GetWatchedList(Paging Params)
        {
            var user = await _iaccountService.GetUserByUserClaim(HttpContext.User);
            var vedios = await _iStudentWatchesService.GetWatchedListAsync(user, Params);
            Response.AddPagination(vedios.CurrentPage, vedios.ItemsPerPage, vedios.TotalItems, vedios.TotalPages);
            return _mapper.Map<List<StudentWatchedVedio>, List<WatchedVedioOutput>>(vedios);
        }
        [Authorize]
        [HttpPost("AddTolist")]
        public async Task<ActionResult<ResultService<bool>>> AddTolist(int VedioId) =>
            GetResult<bool>(await _iStudentWatchesService.AddToWatchedAsync(VedioId, await _iaccountService.GetUserByUserClaim(HttpContext.User)));
        [Authorize]
        [HttpPost("RemoveFromList")]
        public async Task<ActionResult<ResultService<bool>>> RemoveFromList(int VedioId) =>
            GetResult<bool>(await _iStudentWatchesService.RemoveFromWatchedAsync(VedioId, await _iaccountService.GetUserByUserClaim(HttpContext.User)));

    }
}