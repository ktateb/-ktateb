using System.Threading.Tasks;
using DAL.Entities.Comments;
using System.Collections.Generic;
using DAL.Repositories;
using DAL.Entities.Courses;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Model.Helper;
using AutoMapper;
using Model.Comment.Outputs;
using DAL.Entities.Identity;
using Model.SubComment.Outputs;
using System.ComponentModel;
using Model.Comment.Inputs;
using Common.Services;

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



        /*
            Check ReportService to see what i did
        */
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
            resultService.Code = ResultStatusCode.Ok;
            resultService.Result = comment;
            return resultService;
        }

        public async Task<PagedList<SubComment>> GetSubCommentsAsync(int Id, Paging Params)
        {
            var Query = _subcommentrepository.GetQuery().Where(c => c.CommentId == Id).Include(s => s.User).OrderByDescending(c => c.DateComment);
            return await PagedList<SubComment>.CreatePagingListAsync(Query, Params.PageNumber, Params.PageSize);
        }
        public async Task<ResultService<string>> CreateAsync(CommentCreateInput comment, User user)
        {
            var result = new ResultService<string>();
            var commentToCreate = _mapper.Map<CommentCreateInput, Comment>(comment);
            commentToCreate.UserId = user.Id;
            commentToCreate.IsUpdated = false;
            commentToCreate.DateComment = DateTime.Now;
            if (await _commentrepository.CreateAsync(commentToCreate))
            {
                result.Code = ResultStatusCode.Ok;
                result.Messege = "Done";
                result.Result = "Done";
                return result;
            }
            result.Code = ResultStatusCode.InternalServerError;
            result.Messege = "Ex";
            result.Result = "ex";
            return result;
        }

        public async Task<ResultService<string>> UpdateAsync(CommentUpdateInput comment, User user)
        {
            var commentToCreate = _mapper.Map<CommentUpdateInput, Comment>(comment);
            var Result = await CanEdite(user, comment.Id);
            if (Result.Code != ResultStatusCode.Ok)
            {
                return Result;
            }
            var com = await _commentrepository.GetQuery().Where(c => c.Id == comment.Id).Select(c => new { c.DateComment, c.CourseId }).FirstOrDefaultAsync();
            commentToCreate.UserId = user.Id;
            commentToCreate.IsUpdated = true;
            commentToCreate.DateComment = com.DateComment;
            commentToCreate.CourseId = com.CourseId;
            if (await _commentrepository.UpdateAsync(commentToCreate))
                return Result;
            Result.Code = ResultStatusCode.BadRequist;
            Result.Messege = "Cannot update this Comment";
            return Result;
        }
        private async Task<string> GetUserIdOrDefultAsync(int Id) =>
             await _commentrepository.GetQuery().Where(c => c.Id == Id).Select(c => c.UserId).FirstOrDefaultAsync();


        public async Task<ResultService<string>> CanEdite(User user, int CommentId)
        {
            var UserId = await GetUserIdOrDefultAsync(CommentId);
            var Result = new ResultService<string>();
            if (UserId is null)
            {
                Result.Code = ResultStatusCode.NotFound;
                Result.Messege = Result.Result = "Comment Not Found";

            }
            else if (!UserId.Equals(user.Id))
            {
                Result.Code = ResultStatusCode.BadRequist;
                Result.Result = Result.Messege = "You are  not the Owner";
            }
            return Result;
        }
        public async Task<ResultService<string>> DeleteAsync(int Id, User user)
        {
            var Result = await CanEdite(user, Id);
            if (Result.Code != ResultStatusCode.Ok)
            {
                return Result;
            }
            if (await _commentrepository.DeleteAsync(Id))
                return Result;
            Result.Code = ResultStatusCode.BadRequist;
            Result.Messege = "Cannot Delete this Comment";
            return Result;
        }
    }
    public interface ICommentService
    {

        public Task<ResultService<string>> CanEdite(User user, int CommentId);
        public Task<ResultService<CommentOutput>> GetCommmnetAsync(int Id);
        public Task<PagedList<SubComment>> GetSubCommentsAsync(int Id, Paging Params);
        public Task<ResultService<string>> CreateAsync(CommentCreateInput comment, User user);
        public Task<ResultService<string>> UpdateAsync(CommentUpdateInput comment, User user);
        public Task<ResultService<string>> DeleteAsync(int Id, User user);
    }
}