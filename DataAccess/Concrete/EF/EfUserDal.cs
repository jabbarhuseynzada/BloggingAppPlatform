using Core.DataAccess.Concrete;
using Core.Entities.Concrete;
using DataAccess.Abstract;

namespace DataAccess.Concrete.EF;

public class EfUserDal(BloggingAppDbContext context) : BaseRepository<User, BloggingAppDbContext>(context), IUserDal
{
    public List<OperationClaim> GetClaims(User user) //User user   where UOC.UserId == user.Id
    {
        //UserOperations = UserOperationClaims
        using var context = new BloggingAppDbContext();
        var result = from OC in context.OperationClaims
                     join UOC in context.UserOperations
                     on OC.Id equals UOC.OperationClaimId
                     where UOC.UserId == user.Id
                     select new OperationClaim
                     {
                         Id = OC.Id,
                         Name = OC.Name
                     };
        return result.ToList();
    }
}
