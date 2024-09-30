using Core.Entities.Concrete;
using Core.Helpers.Results.Abstract;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserService
    {
        IResult DeleteUser(int id);
        IResult ActivateUser(int id);
        IResult AddOperationClaimToUser(int userId, int operationClaimId);
        List<OperationClaim> GetClaims(User user);
        void Add(User user);
        User GetUserById(int userId);
        User GetByMail(string email);
        User GetByUsername(string username);
        IDataResult<List<UserDto>> GetAllUsers();
        IResult FollowUser(int followedUserId);
        IResult UnfollowUser(int followedUserId);
    }
}