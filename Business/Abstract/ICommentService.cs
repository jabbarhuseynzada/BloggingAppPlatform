using Core.Helpers.Results.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICommentService
    {
        IResult Add(CommentDto comment);
        IResult Update(UpdateCommentDto comment);
        IResult Delete(int commentId);
        IDataResult<List<CommentDto>> GetCommentsByUserId(int userId);
        IDataResult<CommentDto> GetCommentById(int commentId);
        IDataResult<List<CommentDto>> GetAll();
    }
}
