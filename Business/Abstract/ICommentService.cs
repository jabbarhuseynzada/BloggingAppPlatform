using Core.Helpers.Results.Abstract;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface ICommentService
    {
        IResult Add(CommentDto comment);
        IResult Update(UpdateCommentDto comment, int userId);
        IResult Delete(int commentId, int userId);
        IDataResult<List<CommentDto>> GetCommentsByUserId(int userId);
        IDataResult<List<GetCommentDto>> GetCommentsByPostId(int postId);
        IDataResult<CommentDto> GetCommentById(int commentId);
        IDataResult<List<CommentDto>> GetAll();
        IDataResult<List<GetCommentDto>> GetAllComments();
    }
}
