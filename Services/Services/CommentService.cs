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
        private readonly IGenericRepository<Course> _courserepository;
        private readonly IGenericRepository<SubComment> _subcommentrepository;
        private readonly IMapper _mapper;
        public CommentService(IGenericRepository<Course> courserepository, IGenericRepository<Comment> commentrepository, IGenericRepository<SubComment> subcommentrepository, IMapper mapper)
        {
            _courserepository = courserepository;
            _commentrepository = commentrepository;
            _subcommentrepository = subcommentrepository;
            _mapper = mapper;
        }
        public async Task<ResultService<CommentOutput>> GetCommmnetAsync(int Id)
        {
            var result = new ResultService<CommentOutput>();
            var comment = _mapper.Map<Comment, CommentOutput>(await _commentrepository.GetQuery().Include(c => c.User).Where(c => c.Id == Id).FirstOrDefaultAsync());
            if (comment is null)
            {
                result.ErrorField = nameof(Id);
                result.Code = ResultStatusCode.NotFound;
                result.Messege = "Comment is not exist";
                return result;
            }
            result.Code = ResultStatusCode.Ok;
            result.Result = comment;
            return result;
        }

        public async Task<PagedList<SubComment>> GetSubCommentsAsync(int Id, Paging Params)
        {
            var Query = _subcommentrepository.GetQuery().Where(c => c.CommentId == Id).Include(s => s.User).OrderByDescending(c => c.DateComment);
            return await PagedList<SubComment>.CreatePagingListAsync(Query, Params.PageNumber, Params.PageSize);
        }
        public async Task<ResultService<CommentOutput>> CreateAsync(CommentCreateInput comment, User user)
        {
            try
            {
                var result = new ResultService<CommentOutput>();
                var commentToCreate = _mapper.Map<CommentCreateInput, Comment>(comment);
                commentToCreate.UserId = user.Id;
                commentToCreate.IsUpdated = false;
                commentToCreate.DateComment = DateTime.Now;
                if (!await _courserepository.IsExist(commentToCreate.CourseId))
                {
                    result.Code = ResultStatusCode.BadRequest;
                    result.ErrorField = nameof(comment.CourseId);
                    result.Messege = "course not Exist"; 
                }else if (await _commentrepository.CreateAsync(commentToCreate))
                {
                    result.Code = ResultStatusCode.Ok;
                    result.Result = _mapper.Map<Comment, CommentOutput>(commentToCreate);
                    result.Messege = "Done"; 
                }else
                {
                result.Code = ResultStatusCode.BadRequest;
                result.Messege = "Cannot Comment"; 
                }
                return result;
            }
            catch
            {
                return ResultService<CommentOutput>.GetErrorResult();
            }
        }

        public async Task<ResultService<CommentOutput>> UpdateAsync(CommentUpdateInput comment, User user)
        {

            try
            {

                var commentToUpdate = _mapper.Map<CommentUpdateInput, Comment>(comment);
                var result = await CanEdite<CommentOutput>(user, comment.Id);
                if (result.Code != ResultStatusCode.Ok)
                {
                    return result;
                } 
                var com = await _commentrepository.GetQuery().Where(c => c.Id == comment.Id).Select(c => new { c.DateComment, c.CourseId }).FirstOrDefaultAsync();
                commentToUpdate.UserId = user.Id;
                commentToUpdate.IsUpdated = true;
                commentToUpdate.DateComment = com.DateComment;
                commentToUpdate.CourseId = com.CourseId;
                if (await _commentrepository.UpdateAsync(commentToUpdate))
                {
                    result.Result = _mapper.Map<Comment, CommentOutput>(commentToUpdate);
                    result.Messege = "Done";
                    return result;

                }
                result.Code = ResultStatusCode.BadRequest;
                result.Messege = "Cannot update this Comment";
                return result;
            }
            catch
            {
                return ResultService<CommentOutput>.GetErrorResult();
            }
        }
        private async Task<string> GetUserIdOrDefultAsync(int Id) =>
             await _commentrepository.GetQuery().Where(c => c.Id == Id).Select(c => c.UserId).FirstOrDefaultAsync();


        public async Task<ResultService<T>> CanEdite<T>(User user, int CommentId)
        {
            var UserId = await GetUserIdOrDefultAsync(CommentId);
            var Result = new ResultService<T>();
            if (UserId is null)
            {
                Result.Code = ResultStatusCode.NotFound;
                Result.Messege = "Comment Not Found";

            }
            else if (!UserId.Equals(user.Id))
            {
                Result.Code = ResultStatusCode.Unauthorized;
                Result.Messege = "You are  not the Owner";
            }
            return Result;
        }
        public async Task<ResultService<bool>> DeleteAsync(int Id, User user)
        {
            try
            {

                var result = await CanEdite<bool>(user, Id);
                if (result.Code != ResultStatusCode.Ok)
                {
                    return result;
                }
                if (await _commentrepository.DeleteAsync(Id))
                {
                    result.Result = true;
                    return result;
                }
                result.Code = ResultStatusCode.BadRequest;
                result.Messege = "Cannot Delete this Comment";
                return result;
            }
            catch
            {
                return ResultService<bool>.GetErrorResult().SetResult(false);
            }
        }
    }
    public interface ICommentService
    {
        public Task<ResultService<CommentOutput>> GetCommmnetAsync(int Id);
        public Task<PagedList<SubComment>> GetSubCommentsAsync(int Id, Paging Params);
        public Task<ResultService<CommentOutput>> CreateAsync(CommentCreateInput comment, User user);
        public Task<ResultService<CommentOutput>> UpdateAsync(CommentUpdateInput comment, User user);
        public Task<ResultService<bool>> DeleteAsync(int Id, User user);
    }
}