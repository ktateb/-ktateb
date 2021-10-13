using System.Threading.Tasks;
using AutoMapper;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.User.Inputs;
using Model.User.Outputs;
using Services;

namespace API.Controllers.Common
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserOutput>> Login(UserLoginInput input)
        {
            var dbRecord = await _accountService.GetUserByEmailAsync(input.Email);
            if (dbRecord == null)
                dbRecord = await _accountService.GetUserByNameAsync(input.UserName);
            if (dbRecord == null)
                return NotFound("Not found user");
            var result = await _accountService.LoginUser(dbRecord, input.Password);
            if (!result)
                return NotFound("Username or password is wrong");

            var roles = await _accountService.GetRolesByUserIdAsync(dbRecord.Id);
            return _mapper.Map<User, UserOutput>(dbRecord);
        }
    }
}