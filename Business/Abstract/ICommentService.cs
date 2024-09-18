using Core.Helpers.Results.Abstract;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface ICommentService
    {
        IResult Add(CommentDto comment);
        IResult Update(UpdateCommentDto comment);
        IResult Delete(int commentId);
        IDataResult<List<CommentDto>> GetCommentsByUserId(int userId);
        IDataResult<List<CommentDto>> GetCommentsByPostId(int postId);
        IDataResult<CommentDto> GetCommentById(int commentId);
        IDataResult<List<CommentDto>> GetAll();
    }
}
