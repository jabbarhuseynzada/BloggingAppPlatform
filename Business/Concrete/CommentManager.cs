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
        public CommentManager(ICommentDal commentDal, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _commentDal = commentDal;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }
        private readonly ICommentDal _commentDal;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
       
        [SecuredOperation("User,Admin")]
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
        [SecuredOperation("User,Admin")]
        public IResult Delete(int commentId)
        {
            var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            var deleteComment = _commentDal.Get(c => c.Id == commentId && c.IsDeleted == false);
            if(deleteComment != null && (deleteComment.UserId == int.Parse(userId) || userRole == "Admin"))
            {
                deleteComment.IsDeleted = false;
                deleteComment.UpdateTime = DateTime.Now;
                _commentDal.Delete(deleteComment);
                return new SuccessResult("Comment is successfully deleted");
            }
            else
            {
                return new ErrorResult("You do not have permission do delete comment or comment is not exist");
            }
        }
        [SecuredOperation("User,Admin")]
        [ValidationAspect<UpdateCommentDto>(typeof(UpdateCommentValidator))]
        public IResult Update(UpdateCommentDto comment)
        {
            var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            Comment updateComment = _mapper.Map<Comment>(comment);
            if (updateComment != null && (updateComment.UserId == int.Parse(userId) || userRole == "Admin"))
            {
                updateComment.IsDeleted = false;
                updateComment.UpdateTime = DateTime.Now;
                _commentDal.Update(updateComment);
            }
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

    }
}
