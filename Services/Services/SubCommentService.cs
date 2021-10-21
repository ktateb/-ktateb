using System.Threading.Tasks;
using DAL.Entities.Comments;
using System.Collections.Generic;
using DAL.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace Services
{
    public class SubCommentService : ISubCommentService
    {
        private readonly IGenericRepository<SubComment> _SubCommentrepository;

        public SubCommentService(IGenericRepository<SubComment> subCommentrepository)
        {
            _SubCommentrepository = subCommentrepository;
        }

        public async Task<bool> DeleteAsync(int Id) =>
            await _SubCommentrepository.DeleteAsync(Id);

        public async Task<SubComment> GetSubCommmnetAsync(int Id) =>
            await _SubCommentrepository.FindAsync(Id);
        public async Task<string> GetUserIdOrDefultAsync(int Id) =>
            await _SubCommentrepository.GetQuery().Where(c => c.Id == Id).Select(c => c.UserId).FirstOrDefaultAsync();
        public async Task<bool> CreateAsync(SubComment SubComment)
        {
            SubComment.IsUpdated = false;
            SubComment.DateComment = DateTime.Now;
            return await _SubCommentrepository.CreateAsync(SubComment);
        }

        public async Task<bool> UpdateAsync(SubComment SubComment)
        {
            SubComment.IsUpdated = true;
            var com = await _SubCommentrepository.GetQuery().Where(c => c.Id == SubComment.Id).Select(c => new { c.DateComment, c.CommentId }).FirstOrDefaultAsync();
            if(com==default)return false;
            SubComment.DateComment = com.DateComment;
            SubComment.CommentId = com.CommentId;
            return await _SubCommentrepository.UpdateAsync(SubComment);
        }
        public async Task<bool> IsTheOwner(string userid, int SubCommentId) =>
            (await GetUserIdOrDefultAsync(SubCommentId)).Equals(userid);

    }
    public interface ISubCommentService
    {
        public Task<string> GetUserIdOrDefultAsync(int Id);
        public Task<bool> IsTheOwner(string userid, int SubCommentId);
        public Task<SubComment> GetSubCommmnetAsync(int Id);
        public Task<bool> CreateAsync(SubComment SubComment);
        public Task<bool> UpdateAsync(SubComment SubComment);
        public Task<bool> DeleteAsync(int id);
    }
}