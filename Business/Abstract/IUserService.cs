using Core.Entities.Concrete;
using Core.Helpers.Results.Abstract;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserService
    {
        IResult DeleteUser(int id);
        IResult UpdateUser(UpdateUserDto userDto, int userId);
        //IResult ActivateUser(int id);
        IResult AddOperationClaimToUser(string username, string operationClaimName);
        IDataResult<List<OperationClaim>> GetAllOperationClaims();
        List<OperationClaim> GetClaims(User user);
        void Add(User user);
        GetUserDto GetUserById(int userId);
        User GetByMail(string email);
        User GetByUsername(string username);
        IDataResult<List<UserDto>> GetAllUsers();
        IResult FollowUser(int followerId, int followedUserId);
        IResult UnfollowUser(int followerId, int unfollowedUserId);
        IDataResult<bool> IsFollow(int followerId, int followedUserId);
    }
}