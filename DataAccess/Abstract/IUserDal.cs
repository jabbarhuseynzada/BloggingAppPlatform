using Core.DataAccess.Abstract;
using Core.Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Abstract
{
    public interface IUserDal : IBaseRepository<User>
    {
        List<OperationClaim> GetClaims(User user);
        List<UserDto> GetUsers();
    }
}