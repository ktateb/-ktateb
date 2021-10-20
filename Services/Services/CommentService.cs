using System.Threading.Tasks;
using DAL.Entities.Comments;
using System.Collections.Generic;
using DAL.Repositories;
using DAL.Entities.Courses;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Model.Helper;

namespace Services
{
    public class CommentService : ICommentService
    {
        private readonly IGenericRepository<Comment> _commentrepository;
        private readonly IGenericRepository<SubComment> _subcommentrepository;
        public CommentService(IGenericRepository<Comment> commentrepository,IGenericRepository<SubComment> subcommentrepository)
        {
            _commentrepository = commentrepository;
            _subcommentrepository=subcommentrepository;
        }

        public async Task<bool> DeleteAsync(int Id) =>
            await _commentrepository.DeleteAsync(Id);

        public async Task<Comment> GetCommmnetAsync(int Id) =>
            await _commentrepository.GetQuery().Include(c=>c.User).Where(c=>c.Id==Id).FirstOrDefaultAsync();

        public async Task<PagedList<SubComment>> GetSubCommentsAsync(int Id,Paging Params) {
            var q= _subcommentrepository.GetQuery().Where(c => c.CommentId == Id).Include(s=>s.User).OrderByDescending(c=>c.DateComment);
            return await PagedList<SubComment>.CreatePagingListAsync(q, Params.PageNumber, Params.PageSize);
        }
        public async Task<bool> CreateAsync(Comment comment)
        {
            comment.IsUpdated = false;
            comment.DateComment = DateTime.Now;
            return await _commentrepository.CreateAsync(comment);
        }

        public async Task<bool> UpdateAsync(Comment comment)
        {
            comment.IsUpdated = true;
            var com= await _commentrepository.GetQuery().Where(c => c.Id == comment.Id).Select(c => new{c.DateComment,c.CourseId}).FirstOrDefaultAsync();
            if(com==default)return false;
            comment.DateComment = com.DateComment;
            comment.CourseId=com.CourseId;
            return await _commentrepository.UpdateAsync(comment);
        }
        public async Task<String> GetUserIdOrDefultAsync(int Id) =>
            await _commentrepository.GetQuery().Where(c => c.Id == Id).Select(c => c.UserId).FirstOrDefaultAsync();

        public async Task<bool> isTheOwner(String userid,int CommentId)=>
             (await GetUserIdOrDefultAsync(CommentId)).Equals(userid); 


    }
    public interface ICommentService
    { 
        public Task<String> GetUserIdOrDefultAsync(int Id);
        public Task<bool> isTheOwner(String userid,int CommentId);
        public Task<Comment> GetCommmnetAsync(int Id);
        public Task<PagedList<SubComment>> GetSubCommentsAsync(int Id,Paging Params);
        public Task<bool> CreateAsync(Comment comment);
        public Task<bool> UpdateAsync(Comment comment);
        public Task<bool> DeleteAsync(int Id);
    }
}