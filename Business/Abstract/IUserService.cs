using Core.Entities.Concrete;
using Core.Helpers.Results.Abstract;

namespace Business.Abstract
{
    public interface IUserService
    {
        IResult AddOperationClaimToUser(int userId, int operationClaimId);
        List<OperationClaim> GetClaims(User user);
        void Add(User user);
        User GetByMail(string email);
    }
}