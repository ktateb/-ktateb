using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Services;
using DAL.Entities.Identity;
using DAL.Entities.Messages;
using DAL.Repositories;
using Model.Helper;
using Model.Message.Inputs;
using Model.Message.Outputs;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IIdentityRepository _identityRepository;
        private readonly IMapper _mapper;

        public MessageService(IGenericRepository<Message> messageRepository, IIdentityRepository identityRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _identityRepository = identityRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<Message>> GetMessages(MessageParam messageParams, User user)
        {

            var query = _messageRepository.GetQuery()
                .Where(x => (x.SenderId == user.Id && x.ReciverId == messageParams.UserReciverId) ||
                 (x.SenderId == messageParams.UserReciverId && x.ReciverId == user.Id))
                 .OrderBy(x => x.DateSent)
                .AsQueryable();

            if (messageParams.IsDeleted != null)
                query = query.Where(x => x.IsDeleted == messageParams.IsDeleted);
            if (messageParams.RecipentDeleted != null)
                query = query.Where(x => x.RecipentDeleted == messageParams.RecipentDeleted);
            if (messageParams.SenderDeleted != null)
                query = query.Where(x => x.SenderDeleted == messageParams.SenderDeleted);
            return await PagedList<Message>.CreatePagingListAsync(query, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<ResultService<bool>> SendMessage(MessageInput input, User user)
        {
            var result = new ResultService<bool>();
            try
            {
                if (user == null)
                {
                    result.Code = ResultStatusCode.Unauthorized;
                    result.Messege = "You are Unauthorized";
                    result.Result = false;
                    return result;
                }
                var dbRecordUser = await _identityRepository.GetUserByIdAsync(input.ReciverId);
                if (dbRecordUser == null)
                {
                    result.Code = ResultStatusCode.NotFound;
                    result.Messege = "You send message to user not exist";
                    result.Result = false;
                    return result;
                }
                if (user.Id == input.ReciverId)
                {
                    result.Code = ResultStatusCode.BadRequest;
                    result.Messege = @"You can't talking to yourself";
                    result.Result = false;
                    return result;
                }
                var message = _mapper.Map<MessageInput, Message>(input);
                message.SenderId = user.Id;
                message.DateSent = DateTime.UtcNow;
                await _messageRepository.CreateAsync(message);
                result.Code = ResultStatusCode.Ok;
                result.Result = true;
                result.Messege = "Success";
                return result;
            }
            catch
            {
                result.Code = ResultStatusCode.InternalServerError;
                result.Result = false;
                result.Messege = "Exception happen when send message";
                return result;
            }
        }


        public async Task<ResultService<bool>> DeleteMessageForAll(int id, User user)
        {
            var result = new ResultService<bool>();
            try
            {
                if (user == null)
                {
                    result.Code = ResultStatusCode.Unauthorized;
                    result.Messege = "You are Unauthorized";
                    result.Result = false;
                    return result;
                }
                var dbRecord = await _messageRepository.FindAsync(id);
                if (dbRecord == null)
                {
                    result.Code = ResultStatusCode.NotFound;
                    result.Messege = "Message not found";
                    result.Result = false;
                    return result;
                }
                if (dbRecord.SenderId != user.Id)
                {
                    result.Code = ResultStatusCode.Unauthorized;
                    result.Messege = "You are Unauthorized";
                    result.Result = false;
                    return result;
                }
                dbRecord.IsDeleted = true;
                await _messageRepository.UpdateAsync(dbRecord);
                result.Code = ResultStatusCode.Ok;
                result.Result = true;
                result.Messege = "Success";
                return result;
            }
            catch
            {
                result.Code = ResultStatusCode.InternalServerError;
                result.Result = false;
                result.Messege = "Exception happen when delete message";
                return result;
            }
        }

        public async Task<Message> GetMessage(int id) =>
            await _messageRepository.FindAsync(id);

        public async Task<ResultService<bool>> UpdateMessage(UpdateMessageInput input, User user)
        {
            var result = new ResultService<bool>();
            try
            {
                if (user == null)
                {
                    result.Code = ResultStatusCode.Unauthorized;
                    result.Messege = "You are Unauthorized";
                    result.Result = false;
                    return result;
                }
                var dbRecordUser = await _identityRepository.GetUserByIdAsync(input.ReciverId);
                if (dbRecordUser == null)
                {
                    result.Code = ResultStatusCode.NotFound;
                    result.Messege = "User you need to update message with him is not exist";
                    result.Result = false;
                    return result;
                }
                var dbRecordMessage = await _messageRepository.FindAsync(input.Id);
                var message = _mapper.Map<UpdateMessageInput, Message>(input, dbRecordMessage);
                message.SenderId = user.Id;
                message.IsUpdated = true;
                await _messageRepository.UpdateAsync(message);
                result.Code = ResultStatusCode.Ok;
                result.Result = true;
                result.Messege = "Success";
                return result;
            }
            catch
            {
                result.Code = ResultStatusCode.InternalServerError;
                result.Result = false;
                result.Messege = "Exception happen when update message";
                return result;
            }
        }

        public async Task<ResultService<bool>> DeleteMessageForMe(int id, User user)
        {
            var result = new ResultService<bool>();
            try
            {
                if (user == null)
                {
                    result.Code = ResultStatusCode.Unauthorized;
                    result.Messege = "You are Unauthorized";
                    result.Result = false;
                    return result;
                }
                var dbRecord = await _messageRepository.FindAsync(id);
                if (dbRecord == null)
                {
                    result.Code = ResultStatusCode.NotFound;
                    result.Messege = "Message not found";
                    result.Result = false;
                    return result;
                }
                dbRecord.SenderDeleted = true;
                await _messageRepository.UpdateAsync(dbRecord);
                result.Code = ResultStatusCode.Ok;
                result.Result = true;
                result.Messege = "Success";
                return result;
            }
            catch
            {
                result.Code = ResultStatusCode.InternalServerError;
                result.Result = false;
                result.Messege = "Exception happen when delete message";
                return result;
            }
        }
    }
    public interface IMessageService
    {
        public Task<ResultService<bool>> UpdateMessage(UpdateMessageInput input, User user);
        public Task<ResultService<bool>> DeleteMessageForAll(int id, User user);
        public Task<ResultService<bool>> DeleteMessageForMe(int id, User user);
        public Task<Message> GetMessage(int id);
        public Task<PagedList<Message>> GetMessages(MessageParam messageParams, User user);
        public Task<ResultService<bool>> SendMessage(MessageInput input, User user);
    }
}