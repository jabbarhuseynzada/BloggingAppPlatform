using Core.DataAccess.Concrete;
using Core.Entities.Concrete;
using DataAccess.Abstract;

namespace DataAccess.Concrete.EF;

public class EfUserOperationClaimDal(BloggingAppDbContext context) : BaseRepository<UserOperationClaim, BloggingAppDbContext>(context), IUserOperationClaimDal
{
}
