using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.User.Inputs;
using Model.User.Outputs;
using Services;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet("GetUsers")]
        public async Task<List<UsersOutput>> GetUsers() =>
            _mapper.Map<List<User>, List<UsersOutput>>(await _accountService.GetUsers());

        [HttpPost("UpdateUser")]
        public async Task<ActionResult> UpdateUser(UserUpdateInput input)
        {
            var dbRecord = await _accountService.GetUserByIdAsync(input.Id);
            if (dbRecord == null)
                return NotFound("This user not exist");
            if (await _accountService.UpdateUser(_mapper.Map<UserUpdateInput, User>(input)))
                return Ok("Done");
            return BadRequest("Error happen when update user");
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var dbRecord = await _accountService.GetUserByIdAsync(id);
            if (dbRecord == null)
                return NotFound("This user not exist");
            if (await _accountService.DeleteUser(id))
                return Ok("Done");
            return BadRequest("Error happen when delete user");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserOutput>> Login(UserLoginInput input)
        {
            var dbRecord = await _accountService.GetUserByEmailAsync(input.UserNameOrEmail);
            if (dbRecord == null)
                dbRecord = await _accountService.GetUserByNameAsync(input.UserNameOrEmail);
            if (dbRecord == null)
                return NotFound("Not found user");
            var result = await _accountService.LoginUser(dbRecord, input.Password);
            if (!result)
                return NotFound("Username or password is wrong");

            var data = _mapper.Map<User, UserOutput>(dbRecord);
            data.Token = _tokenService.CreateToken(dbRecord);
            return Ok(data);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserOutput>> Register(UserRegisterInput input)
        {
            var dbRecord = await _accountService.GetUserByNameAsync(input.UserName);
            if (dbRecord != null)
                return BadRequest("This username is not used by another user");
            dbRecord = await _accountService.GetUserByEmailAsync(input.Email);
            if (dbRecord != null)
                return BadRequest("This email is not used by another user");
            var result = await _accountService.RegisterUser(input);
            if (!result)
                return BadRequest("Something wrong when you regester");
            dbRecord = await _accountService.GetUserByNameAsync(input.UserName);
            var data = _mapper.Map<User, UserOutput>(dbRecord);
            data.Token = _tokenService.CreateToken(dbRecord);
            return Ok(data);
        }
    }
}