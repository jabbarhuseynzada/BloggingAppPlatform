using AutoMapper;
using Business.Abstract;
using Business.BusinessAspect.Autofac.Secured;
using Business.Validation.FluentValidation;
using Core.Aspects.Autofac.Validation.FluentValidation;
using Core.Extensions;
using Core.Helpers.Results.Abstract;
using Core.Helpers.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using IResult = Core.Helpers.Results.Abstract.IResult;

namespace Business.Concrete
{
    public class CommentManager : ICommentService
    {
        public CommentManager(ICommentDal commentDal, IHttpContextAccessor contextAccessor, IMapper mapper, IPostDal postDal)
        {
            _commentDal = commentDal;
            _postDal = postDal;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }
        private readonly ICommentDal _commentDal;
        private readonly IPostDal _postDal;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        [SecuredOperation("User,Admin,Moderator")]
        [ValidationAspect<CommentDto>(typeof(CommentValidator))]
        public IResult Add(CommentDto comment)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            Comment addComment = _mapper.Map<Comment>(comment);

            addComment.UpdateTime = DateTime.Now;
            addComment.CreateDate = DateTime.Now;
            addComment.IsDeleted = false;
            addComment.UserId = int.Parse(userId);
            _commentDal.Add(addComment);
            return new SuccessResult("Comment added successfully");
        }
        [SecuredOperation("User,Admin,Moderator")]
        public IResult Delete(int commentId)
        {
            var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var deleteCommentClaim = _contextAccessor.HttpContext.User.ClaimRoles().Contains("comment.delete");
            var deleteComment = _commentDal.Get(c => c.Id == commentId && c.IsDeleted == false);
            if (deleteComment != null && (deleteComment.UserId == int.Parse(userId) || _postDal.Get(p => p.Id == deleteComment.PostId).UserId == int.Parse(userId) || deleteCommentClaim == true))
            {
                deleteComment.IsDeleted = true;
                deleteComment.UpdateTime = DateTime.Now;
                _commentDal.Delete(deleteComment);
                return new SuccessResult("Comment is successfully deleted");
            }
            else
            {
                return new ErrorResult("You do not have permission to delete comment");
            }
        }
        [SecuredOperation("User,Admin,Moderator")]
        [ValidationAspect<UpdateCommentDto>(typeof(UpdateCommentValidator))]
        public IResult Update(UpdateCommentDto comment)
        {
            var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            Comment updateComment = _mapper.Map<Comment>(comment);
            if (updateComment != null && updateComment.UserId == int.Parse(userId))
            {
                updateComment.IsDeleted = false;
                updateComment.UpdateTime = DateTime.Now;
                _commentDal.Update(updateComment);
                return new SuccessResult("Comment Succesfully uptaded");
            }
            else
            {
                return new ErrorResult("Problem occured while comment updating");
            }
        }
        public IDataResult<List<CommentDto>> GetAll()
        {
            var comments = _commentDal.GetAll(c => c.IsDeleted == false);
            var commentDtos = _mapper.Map<List<CommentDto>>(comments);
            if (comments.Count > 0)
                return new SuccessDataResult<List<CommentDto>>(commentDtos, "All comments are fetched");
            else
                return new ErrorDataResult<List<CommentDto>>(commentDtos, "There is no comments");
        }

        public IDataResult<CommentDto> GetCommentById(int commentId)
        {
            var comment = _commentDal.Get(c => c.Id == commentId && c.IsDeleted == false);
            var commentDto = _mapper.Map<CommentDto>(comment);
            if (commentDto != null)
            {
                return new SuccessDataResult<CommentDto>(commentDto, "Comment is fetched");
            }
            else return new ErrorDataResult<CommentDto>(commentDto, "There is no such comment");
            throw new NotImplementedException();
        }

        public IDataResult<List<CommentDto>> GetCommentsByUserId(int userId)
        {
            var comments = _commentDal.GetAll(c => c.UserId == userId && c.IsDeleted == false);
            var commentDtos = _mapper.Map<List<CommentDto>>(comments);
            if (commentDtos.Count > 0)
                return new SuccessDataResult<List<CommentDto>>(commentDtos, "Comment sucessfully fetched");
            else
                return new ErrorDataResult<List<CommentDto>>(commentDtos, "An error occured while posts were fetching");
        }

        public IDataResult<List<CommentDto>> GetCommentsByPostId(int postId)
        {
            var comments = _commentDal.GetAll(c => c.PostId == postId && c.IsDeleted == false);
            var commentDtos = _mapper.Map<List<CommentDto>>(comments);
            if (commentDtos.Count > 0)
                return new SuccessDataResult<List<CommentDto>>(commentDtos, "Comment sucessfully fetched");
            else
                return new ErrorDataResult<List<CommentDto>>(commentDtos, "An error occured while posts were fetching");
        }
    }
}
