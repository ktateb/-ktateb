using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities.Messages;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Message>> GetMessages(string userId, MessageParam messageParamss)
        {
            var query = _messageRepository.GetQuery()
                .Where(x => (x.SenderId == userId && x.ReciverId == messageParamss.UserReciverId) ||
                 (x.SenderId == messageParamss.UserReciverId && x.ReciverId == userId))
                 .OrderBy(x => x.DateSent)
                .AsQueryable();

            if (messageParamss.IsDeleted != null)
                query = query.Where(x => x.IsDeleted == messageParamss.IsDeleted);
            if (messageParamss.RecipentDeleted != null)
                query = query.Where(x => x.RecipentDeleted == messageParamss.RecipentDeleted);
            if (messageParamss.SenderDeleted != null)
                query = query.Where(x => x.SenderDeleted == messageParamss.SenderDeleted);

            return await query.ToListAsync();
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
        public Task<List<Message>> GetMessages(string userId, MessageParam messageParamsss);
        public Task<bool> UpdateMessage(Message message);
        public Task<bool> DeleteMessageForAll(int id);
        public Task<bool> DeleteMessageForMe(int id);
        public Task<bool> SendMessage(Message message);
    }
}