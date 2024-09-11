using Core.DataAccess.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Concrete.EF;

public class EfPostDal(BloggingAppDbContext context) : BaseRepository<Post, BloggingAppDbContext>(context), IPostDal
{
}
