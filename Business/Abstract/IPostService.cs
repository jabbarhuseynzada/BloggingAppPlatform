using Core.Helpers.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IPostService
    {
        IResult Add(AddPostDto post);
        IResult Update(UpdatePostDto post);
        IResult Delete(int id);
        IDataResult<List<Post>> GetPostsByUserId(int userId);
        IDataResult<Post> GetPostById(int id);
        IDataResult<List<Post>> GetAllPosts();
    }
}
