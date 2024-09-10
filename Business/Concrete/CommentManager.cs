using AutoMapper;
using Business.Abstract;
using Business.BusinessAspect.Autofac.Secured;
using Business.Validation.FluentValidation;
using Core.Aspects.Autofac.Validation.FluentValidation;
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
        public CommentManager(ICommentDal commentDal, IHttpContextAccessor contextAccessor)
        {
            _commentDal = commentDal;
            _contextAccessor = contextAccessor;
        }
        private readonly ICommentDal _commentDal;
        private readonly IHttpContextAccessor _contextAccessor;
       
        [SecuredOperation("User,Admin,Moderator")]
        [ValidationAspect<CommentDto>(typeof(CommentValidator))]
        public IResult Add(CommentDto comment)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            Comment addedComment = new()
            {
                UserId = int.Parse(userId.ToString()),
                CommentText = comment.CommentText,
                PostId = comment.PostId,
                CreateDate = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDeleted = false
            };
            _commentDal.Add(addedComment);
            return new SuccessResult("Comment added successfully");
        }

        public IResult Delete(int commentId)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<CommentDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public IDataResult<CommentDto> GetCommentById(int commentId)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<CommentDto>> GetCommentsByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public IResult Update(UpdateCommentDto comment)
        {
            throw new NotImplementedException();
        }
    }
}
