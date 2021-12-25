using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Option;
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

        [HttpGet("User/{username}")]
        public async Task<ActionResult<UserOutput>> GetUser(string username)
        {
            var dbRecord = await _accountService.GetUserByNameAsync(username);
            if (dbRecord == null)
                return NotFound($"User {username} is not exist");
            return Ok(_mapper.Map<User, UserOutput>(dbRecord));
        }

        [Authorize]
        [HttpPost("Update")]
        public async Task<ActionResult> UpdateUser(UserUpdate input)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            if (await _accountService.UpdateUser(_mapper.Map<UserUpdate, User>(input, user)))
                return Ok("Done");
            return BadRequest("Error happen when update user");
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            if (await _accountService.DeleteUser(id))
                return Ok("Done");
            return BadRequest("Error happen when delete user");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserOutput>> Login(UserLogin input)
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
            data.Token = await _tokenService.CreateToken(dbRecord);
            return Ok(data);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserOutput>> Register(UserRegister input)
        {
            var dbRecord = await _accountService.GetUserByNameAsync(input.UserName);
            if (dbRecord != null)
                return BadRequest("This username is used by another user");
            dbRecord = await _accountService.GetUserByEmailAsync(input.Email);
            if (dbRecord != null)
                return BadRequest("This email is used by another user");
            var result = await _accountService.RegisterUser(input);
            if (!result)
                return BadRequest("Something wrong when you regester");
            dbRecord = await _accountService.GetUserByNameAsync(input.UserName);
            var data = _mapper.Map<User, UserOutput>(dbRecord);
            data.Token = await _tokenService.CreateToken(dbRecord);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("UploadImage")]
        public async Task<ActionResult> UploadImage([FromForm] UpdatePhoto input)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var file = input.PictureUrl;
            if (file == null)
                return BadRequest("Please add photo to your product.");
            var path = Path.Combine("wwwroot/images/", "ProfilePhotoFor" + user.UserName + "_" + file.FileName);
            var oldPath = "wwwroot/" + user.PictureUrl;
            if (System.IO.File.Exists(oldPath))
                System.IO.File.Delete(oldPath);
            var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            await stream.DisposeAsync();
            user.PictureUrl = path[7..];
            await _accountService.UpdateUser(user);
            return Ok("Done");
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePassword input)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");
            if (await _accountService.CheckPassword(user, input.CurrentPassword) == false)
                return BadRequest("Please enter your correct password");
            await _accountService.ChangePassword(user, input.CurrentPassword, input.Password);
            return Ok("Done");
        }

        [AllowAnonymous]
        [HttpGet("GetGenders")]
        public List<OptionOutput> GetGenders() =>
            _accountService.GetGenders();

        [Authorize]
        [HttpGet("Me")]
        public async Task<UsernameAndRolesOnly> GetMe() =>
            await _accountService.GetMe(await _accountService.GetUserByUserClaim(HttpContext.User));
    }
}