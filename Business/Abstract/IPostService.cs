using Core.Helpers.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IPostService
    {
        IResult Add(AddPostDto post);
        IResult Update(UpdatePostDto post, int userId);
        IResult Delete(int postId, int userId);
        IDataResult<List<GetPostDto>> GetPostsByUserId(int userId);
        IDataResult<Post> GetPostById(int id);
        IDataResult<List<GetPostDto>> GetAllPosts(int page = 1, int pageSize = 10);
        IDataResult<List<Post>> GetPosts();
        int GetPostCount();
        
    }
}
