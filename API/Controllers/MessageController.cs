using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using DAL.Entities.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public MessageController(IMessageService messageRepository, IAccountService accountService, IMapper mapper)
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
                return Unauthorized("User is Unauthorized");
            if (user.Id == messageParams.UserReciverId)
                return BadRequest(@"You can't get your messages with yourself");

            return Ok(_mapper.Map<List<Message>, List<MessageOutput>>(await _messageRepository.GetMessages(user.Id, messageParams)));
        }

        [HttpDelete("DeleteMessageForAll/{id}")]
        public async Task<ActionResult> DeleteMessageForAll(int id)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null)
                return Unauthorized("User is Unauthorized");
            var dbRecord = await _messageRepository.GetMessage(id);
            if (dbRecord == null)
                return NotFound("Message is not exist");
            if (dbRecord.SenderId != user.Id)
                return Unauthorized("User is Unauthorized");
            await _messageRepository.DeleteMessageForAll(id);
            return Ok("Done");
        }

        [HttpDelete("DeleteMessageForMe/{id}")]
        public async Task<ActionResult> DeleteMessageForMe(int id)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null)
                return Unauthorized("User is Unauthorized");

            var dbRecord = await _messageRepository.GetMessage(id);
            if (dbRecord == null)
                return NotFound("Message is not exist");

            await _messageRepository.DeleteMessageForMe(id);
            return Ok("Done");
        }

        [HttpPost("Update")]
        public async Task<ActionResult<MessageOutput>> UpdateMessage(UpdateMessageInput input)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var dbRecordUser = await _accountService.GetUserByIdAsync(input.ReciverId);
            if (dbRecordUser == null)
                return BadRequest("User you need to update message with him is not exist");

            var dbRecordMessage = await _messageRepository.GetMessage(input.Id);
            var message = _mapper.Map<UpdateMessageInput, Message>(input, dbRecordMessage);
            message.SenderId = user.Id;
            message.IsUpdated = true;
            await _messageRepository.UpdateMessage(message);
            return Ok(_mapper.Map<Message, MessageOutput>(message));
        }

        [HttpPost("SendMessage")]
        public async Task<ActionResult<MessageOutput>> SendMessage(MessageInput input)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");
            var dbRecordUser = await _accountService.GetUserByIdAsync(input.ReciverId);
            if (dbRecordUser == null)
                return BadRequest("You send message to user not exist");
            if (user.Id == input.ReciverId)
                return BadRequest(@"You can't talking to yourself");

            var message = _mapper.Map<MessageInput, Message>(input);
            message.SenderId = user.Id;
            await _messageRepository.SendMessage(message);
            return Ok(_mapper.Map<Message, MessageOutput>(message));
        }
    }
}