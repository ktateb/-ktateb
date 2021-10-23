using System.Threading.Tasks;
using DAL.Entities.Comments;
using System.Collections.Generic;
using DAL.Repositories;
using DAL.Entities.Courses;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Model.Helper;
using Services.Services;
using AutoMapper;
using Model.Comment.Outputs;

namespace Services
{
    public class CommentService : ICommentService
    {
        private readonly IGenericRepository<Comment> _commentrepository;
        private readonly IGenericRepository<SubComment> _subcommentrepository;
        private readonly IMapper _mapper;
        public CommentService(IGenericRepository<Comment> commentrepository, IGenericRepository<SubComment> subcommentrepository, IMapper mapper)
        {
            _commentrepository = commentrepository;
            _subcommentrepository = subcommentrepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteAsync(int Id) =>
            await _commentrepository.DeleteAsync(Id);

        public async Task<ResultService<CommentOutput>> GetCommmnetAsync(int Id)
        {
            var resultService = new ResultService<CommentOutput>();
            var comment = _mapper.Map<Comment, CommentOutput>(await _commentrepository.GetQuery().Include(c => c.User).Where(c => c.Id == Id).FirstOrDefaultAsync());

            if (comment is null)
            {
                resultService.Code = ResultStatusCode.NotFound;
                resultService.Messege = "Comment is not exist";
                return resultService;
            }
            resultService.Code = ResultStatusCode.Done;
            resultService.Result = comment;
            return resultService;
        }

        public async Task<PagedList<SubComment>> GetSubCommentsAsync(int Id, Paging Params)
        {
            var q = _subcommentrepository.GetQuery().Where(c => c.CommentId == Id).Include(s => s.User).OrderByDescending(c => c.DateComment);
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
            var com = await _commentrepository.GetQuery().Where(c => c.Id == comment.Id).Select(c => new { c.DateComment, c.CourseId }).FirstOrDefaultAsync();
            if (com == default) return false;
            comment.DateComment = com.DateComment;
            comment.CourseId = com.CourseId;
            return await _commentrepository.UpdateAsync(comment);
        }
        public async Task<String> GetUserIdOrDefultAsync(int Id) =>
            await _commentrepository.GetQuery().Where(c => c.Id == Id).Select(c => c.UserId).FirstOrDefaultAsync();

        public async Task<bool> IsTheOwner(String userid, int CommentId) =>
             (await GetUserIdOrDefultAsync(CommentId)).Equals(userid);


    }
    public interface ICommentService
    {
        public Task<String> GetUserIdOrDefultAsync(int Id);
        public Task<bool> IsTheOwner(String userid, int CommentId);
        public Task<ResultService<CommentOutput>> GetCommmnetAsync(int Id);
        public Task<PagedList<SubComment>> GetSubCommentsAsync(int Id, Paging Params);
        public Task<bool> CreateAsync(Comment comment);
        public Task<bool> UpdateAsync(Comment comment);
        public Task<bool> DeleteAsync(int Id);
    }
}