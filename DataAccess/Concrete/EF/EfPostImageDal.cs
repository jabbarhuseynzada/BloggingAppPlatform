using Core.DataAccess.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;

namespace DataAccess.Concrete.EF;

public class EfPostImageDal(BloggingAppDbContext context) : BaseRepository<PostImage, BloggingAppDbContext>(context), IPostImageDal
{
}
