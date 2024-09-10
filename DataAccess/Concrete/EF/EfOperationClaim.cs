using Core.DataAccess.Concrete;
using Core.Entities.Concrete;
using DataAccess.Abstract;

namespace DataAccess.Concrete.EF;

public class EfOperationClaim(BloggingAppDbContext context) : BaseRepository<OperationClaim, BloggingAppDbContext>(context), IOperationClaimDal
{
}
