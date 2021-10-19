using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities.Messages;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Helper;
using Model.Message.Inputs;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _messageRepository;

        public MessageService(IGenericRepository<Message> messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<bool> DeleteMessageForAll(int id)
        {
            var dbRecord = await _messageRepository.FindAsync(id);
            dbRecord.IsDeleted = true;
            return await _messageRepository.UpdateAsync(dbRecord);
        }

        public async Task<Message> GetMessage(int id) =>
            await _messageRepository.FindAsync(id);

        public async Task<PagedList<Message>> GetMessages(string userId, MessageParam messageParams)
        {
            var query = _messageRepository.GetQuery()
                .Where(x => (x.SenderId == userId && x.ReciverId == messageParams.UserReciverId) ||
                 (x.SenderId == messageParams.UserReciverId && x.ReciverId == userId))
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

        public async Task<bool> SendMessage(Message message) =>
            await _messageRepository.CreateAsync(message);

        public async Task<bool> UpdateMessage(Message message) =>
            await _messageRepository.UpdateAsync(message);

        public async Task<bool> DeleteMessageForMe(int id)
        {
            var dbRecord = await _messageRepository.FindAsync(id);
            dbRecord.SenderDeleted = true;
            return await _messageRepository.UpdateAsync(dbRecord);
        }
    }
    public interface IMessageService
    {
        public Task<Message> GetMessage(int id);
        public Task<PagedList<Message>> GetMessages(string userId, MessageParam messageParams);
        public Task<bool> UpdateMessage(Message message);
        public Task<bool> DeleteMessageForAll(int id);
        public Task<bool> DeleteMessageForMe(int id);
        public Task<bool> SendMessage(Message message);
    }
}