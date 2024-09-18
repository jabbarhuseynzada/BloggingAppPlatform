using Core.DataAccess.Abstract;
using Core.Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IUserDal : IBaseRepository<User>
    {
        List<OperationClaim> GetClaims(User user);
    }
}