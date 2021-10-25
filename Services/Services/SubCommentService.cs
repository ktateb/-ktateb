using System.Threading.Tasks;
using DAL.Entities.Comments;
using System.Collections.Generic;
using DAL.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Model.SubComment.Outputs;
using Common.Services;
using AutoMapper;
using DAL.Entities.Identity;
using Model.SubComment.Inputs;

namespace Services
{
    public class SubCommentService : ISubCommentService
    {
        private readonly IGenericRepository<SubComment> _SubCommentrepository;
        private readonly IGenericRepository<Comment> _Commentrepository;
        private readonly IMapper _mapper;
        public SubCommentService(IGenericRepository<Comment> Commentrepository, IMapper mapper, IGenericRepository<SubComment> subCommentrepository)
        {
            _Commentrepository = Commentrepository;
            _mapper = mapper;
            _SubCommentrepository = subCommentrepository;
        }
        public async Task<ResultService<SubCommentOutput>> GetSubCommmnetAsync(int Id)
        {
            ResultService<SubCommentOutput> result = new();
            var comment = await _SubCommentrepository.FindAsync(Id); ;
            if (comment is null)
            {
                return result.SetCode(ResultStatusCode.NotFound)
                 .SetMessege("Commmnet Not Found").SetErrorField(nameof(Id));
            }
            return result.SetCode(ResultStatusCode.Ok)
                .SetMessege("Ok")
                .SetResult(_mapper.Map<SubComment, SubCommentOutput>(comment));
        }

        public async Task<ResultService<SubCommentOutput>> CreateAsync(SubCommentCreateInput SubCommentInput, User user)
        {
            try
            {

                ResultService<SubCommentOutput> result = new();
                if (!await _Commentrepository.IsExist(SubCommentInput.BaseCommentId))
                {
                    return result.SetCode(ResultStatusCode.BadRequest)
                        .SetErrorField(nameof(SubCommentCreateInput.BaseCommentId))
                        .SetMessege("Comment Not Found");
                }
                var commentToCreate = _mapper.Map<SubCommentCreateInput, SubComment>(SubCommentInput);
                commentToCreate.UserId = user.Id;
                commentToCreate.IsUpdated = false;
                commentToCreate.DateComment = DateTime.Now;
                if (await _SubCommentrepository.CreateAsync(commentToCreate))
                {
                    return result.SetResult(_mapper.Map<SubComment, SubCommentOutput>(commentToCreate));
                }
                return result.SetCode(ResultStatusCode.BadRequest)
                        .SetMessege("SubComment Not Added");
            }
            catch
            {
                return ResultService<SubCommentOutput>.GetErrorResult();
            }
        }

        public async Task<ResultService<SubCommentOutput>> UpdateAsync(SubCommentUpdateInput SubCommentInput, User user)
        {
            try
            {

                ResultService<SubCommentOutput> result = await CanEdit<SubCommentOutput>(SubCommentInput.Id, user);
                if (result.Code != ResultStatusCode.Ok) return result;

                var com = await _SubCommentrepository.GetQuery().Where(c => c.Id == SubCommentInput.Id).Select(c => new { c.DateComment, c.CommentId }).FirstOrDefaultAsync();

                if (com == default) return ResultService<SubCommentOutput>.GetErrorResult();

                var commentToUpdate = _mapper.Map<SubCommentUpdateInput, SubComment>(SubCommentInput);
                commentToUpdate.IsUpdated = true;
                commentToUpdate.DateComment = com.DateComment;
                commentToUpdate.CommentId = com.CommentId;
                commentToUpdate.UserId = user.Id;
                if (await _SubCommentrepository.UpdateAsync(commentToUpdate))
                {
                    return result.SetResult(_mapper.Map<SubComment, SubCommentOutput>(commentToUpdate));
                }
                return result.SetCode(ResultStatusCode.BadRequest).SetMessege("Comment Not added");
            }
            catch
            {
                return ResultService<SubCommentOutput>.GetErrorResult();
            }
        }
        public async Task<ResultService<bool>> DeleteAsync(int Id, User user)
        {
            try
            {

                ResultService<bool> result = await CanEdit<bool>(Id, user);
                if (result.Code != ResultStatusCode.Ok) return result.SetResult(false);

                if (await _SubCommentrepository.DeleteAsync(Id))
                {
                    return result.SetResult(true);
                }
                return result.SetCode(ResultStatusCode.BadRequest).SetResult(false).SetMessege("Comment Not added");

            }
            catch
            {
                return ResultService<bool>.GetErrorResult().SetResult(false);
            }

        }
        private async Task<ResultService<T>> CanEdit<T>(int SubCommentId, User user)
        {
            ResultService<T> result = new();
            var ownerId = await _SubCommentrepository.GetQuery().Where(c => c.Id == SubCommentId).Select(c => c.UserId).FirstOrDefaultAsync();
            if (ownerId == default)
            {
                result.Code = ResultStatusCode.NotFound;
                result.Messege = "Comment Not Found";
                result.ErrorField = "Id";
            }
            else if (!ownerId.Equals(user.Id))
            {
                result.Code = ResultStatusCode.Unauthorized;
                result.Messege = "you are not the owner of the Comment";
                result.ErrorField = "Id";
            }
            return result;

        }

    }
    public interface ISubCommentService
    {
        public Task<ResultService<SubCommentOutput>> GetSubCommmnetAsync(int Id);
        public Task<ResultService<SubCommentOutput>> CreateAsync(SubCommentCreateInput SubCommentInput, User user);
        public Task<ResultService<SubCommentOutput>> UpdateAsync(SubCommentUpdateInput SubCommentInput, User user);
        public Task<ResultService<bool>> DeleteAsync(int Id, User user);
    }
}