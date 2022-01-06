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
using Microsoft.AspNetCore.SignalR;
using Model.Helper;
using Model.Message.Inputs;
using Model.Message.Outputs;
using Services;
using Services.Hubs;

namespace API.Controllers
{
    [Authorize]
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageRepository;
        private readonly IAccountService _accountService;
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IMapper _mapper;

        public MessageController(IMapper mapper, IHubContext<ChatHub> chatHub, IMessageService messageRepository, IAccountService accountService)
        {
            _messageRepository = messageRepository;
            _accountService = accountService;
            _chatHub = chatHub;
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
        public async Task<ActionResult<ResultService<bool>>> UpdateMessage(UpdateMessageInput input)
        {
            await _chatHub.Clients.All.SendAsync("ReceiverOne", input.ReciverId, input.Text);
            return GetResult(await _messageRepository.UpdateMessage(input, await _accountService.GetUserByUserClaim(HttpContext.User)));
        }

        [HttpPost("SendMessage")]
        public async Task<ActionResult<ResultService<bool>>> SendMessage(MessageInput input)
        {
            await _chatHub.Clients.All.SendAsync("ReceiverOne", input.ReciverId, input.Text);
            return GetResult(await _messageRepository.SendMessage(input, await _accountService.GetUserByUserClaim(HttpContext.User)));
        }
    }
}
