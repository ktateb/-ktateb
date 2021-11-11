using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using API.Helpers;
using AutoMapper;
using Common.Services;
using DAL.Entities.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Helper;
using Model.Message.Inputs;
using Model.Message.Outputs;
using Services;

namespace API.Controllers
{
    [Authorize]
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public MessageController(IMapper mapper, IMessageService messageRepository, IAccountService accountService)
        {
            _messageRepository = messageRepository;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("Messages")]
        public async Task<ActionResult<List<MessageOutput>>> GetMessages(MessageParam messageParams)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null)
            {
                return Unauthorized("You are Unauthorized");
            }
            if (user.Id == messageParams.UserReciverId)
            {
                return BadRequest(@"can't get your messages with yourself");
            }
            var messages = await _messageRepository.GetMessages(messageParams, user);
            Response.AddPagination(messages.CurrentPage, messages.ItemsPerPage, messages.TotalItems, messages.TotalPages);
            return _mapper.Map<List<Message>, List<MessageOutput>>(messages);
        }

        [HttpDelete("DeleteMessageForAll/{id}")]
        public async Task<ActionResult<ResultService<bool>>> DeleteMessageForAll(int id) =>
            GetResult(await _messageRepository.DeleteMessageForAll(id, await _accountService.GetUserByUserClaim(HttpContext.User)));

        [HttpDelete("DeleteMessageForMe/{id}")]
        public async Task<ActionResult<ResultService<bool>>> DeleteMessageForMe(int id) =>
            GetResult(await _messageRepository.DeleteMessageForMe(id, await _accountService.GetUserByUserClaim(HttpContext.User)));


        [HttpPost("Update")]
        public async Task<ActionResult<ResultService<bool>>> UpdateMessage(UpdateMessageInput input) =>
            GetResult(await _messageRepository.UpdateMessage(input, await _accountService.GetUserByUserClaim(HttpContext.User)));

        [HttpPost("SendMessage")]
        public async Task<ActionResult<ResultService<bool>>> SendMessage(MessageInput input) =>
            GetResult(await _messageRepository.SendMessage(input, await _accountService.GetUserByUserClaim(HttpContext.User)));
    }
}


// var user = await _accountService.GetUserByUserClaim(HttpContext.User);
//             if (user == null) return Unauthorized("User is Unauthorized");

//             var dbRecordUser = await _accountService.GetUserByIdAsync(input.ReciverId);
//             if (dbRecordUser == null)
//                 return BadRequest("User you need to update message with him is not exist");

//             var dbRecordMessage = await _messageRepository.GetMessage(input.Id);
//             var message = _mapper.Map<UpdateMessageInput, Message>(input, dbRecordMessage);
//             message.SenderId = user.Id;
//             message.IsUpdated = true;
//             await _messageRepository.UpdateMessage(message);
//             return Ok(_mapper.Map<Message, MessageOutput>(message));