using Core.DataAccess.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;

namespace DataAccess.Concrete.EF;

public class EfCommentDal(BloggingAppDbContext context) : BaseRepository<Comment, BloggingAppDbContext>(context), ICommentDal
{

}
