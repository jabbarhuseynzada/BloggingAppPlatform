using Core.DataAccess.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Concrete.EF;

public class EfPostDal(BloggingAppDbContext context) : BaseRepository<Post, BloggingAppDbContext>(context), IPostDal
{
    public List<AddPostDto> GetPostByUserId(int userId)
    {
        var context = new BloggingAppDbContext();
        var result = from p in context.Posts
                     where p.UserId == userId && p.IsDeleted == false
                     select new AddPostDto
                     {
                         UserId = userId,
                         PostTitle = p.Title,
                         PostContext = p.Context,
                         CoverImageUrl = p.CoverImageUrl,
                     };
        return result.ToList();
    }
}
