using Core.Entities.Concrete;

namespace Core.Helpers.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateAccessToken(User user, List<OperationClaim> opeartionClaims);
    }
}
