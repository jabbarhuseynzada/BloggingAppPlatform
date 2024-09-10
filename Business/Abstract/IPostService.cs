using Core.Helpers.Results.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IPostService
    {
        IResult Add(AddPostDto post);
        IResult Update(UpdatePostDto post);
        IResult Delete(int id);
        IDataResult<List<AddPostDto>> GetPostsByUserId(int userId);
        IDataResult<AddPostDto> GetPostById(int id);
        IDataResult<List<AddPostDto>> GetAll();
    }
}
