using AutoMapper;
using Business.Abstract;
using Business.BusinessAspect.Autofac.Secured;
using Core.Entities.Concrete;
using Core.Helpers.Results.Abstract;
using Core.Helpers.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EF;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using IResult = Core.Helpers.Results.Abstract.IResult;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IUserOperationClaimDal _userOperationClaimDal;
        private readonly IMapper _mapper;
        private readonly IOperationClaimDal _operationClaimDal;
        private readonly IUserFollowerDal _userFollowerDal;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserManager(IUserDal userDal, IUserOperationClaimDal userOperationClaimDal, IMapper mapper, IOperationClaimDal operationClaimDal, IUserFollowerDal userFollowerDal, IHttpContextAccessor contextAccessor)
        {
            _userDal = userDal;
            _userOperationClaimDal = userOperationClaimDal;
            _mapper = mapper;
            _operationClaimDal = operationClaimDal;
            _userFollowerDal = userFollowerDal;
            _contextAccessor = contextAccessor;
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public void Add(User user)
        {
            _userDal.Add(user);

            UserOperationClaim claim = new()
            {
                UserId = user.Id,
                OperationClaimId = 1
            };
            _userOperationClaimDal.Add(claim);
        }
        [SecuredOperation("Admin")]
        public IResult AddOperationClaimToUser(int userId, int operationClaimId)
        {
            // Check if the user exists in the User table
            var userExists = _userDal.Get(u => u.Id == userId);
            if (userExists == null)
            {
                return new ErrorResult("User does not exist.");
            }

            // Check if the operation claim exists in the OperationClaim table
            var operationClaimExists = _operationClaimDal.Get(c => c.Id == operationClaimId);
            if (operationClaimExists == null)
            {
                return new ErrorResult("Operation claim does not exist.");
            }

            // Create the UserOperationClaim object
            var claim = new UserOperationClaim()
            {
                UserId = userId,
                OperationClaimId = operationClaimId
            };

            // Check if the user already has the claim
            var checkClaim = _userOperationClaimDal.Get(c => c.UserId == claim.UserId && c.OperationClaimId == claim.OperationClaimId);
            if (checkClaim == null)
            {
                _userOperationClaimDal.Add(claim);
                return new SuccessResult("New operation claim successfully added to user.");
            }
            else
            {
                return new ErrorResult("This user already has this operation claim.");
            }
        }

        public User GetUserById(int userId)
        {
            return _userDal.Get(u => u.Id == userId);
        }


        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }

        public User GetByUsername(string username)
        {
            return _userDal.Get(u => u.Username == username);
        }

        public IDataResult<List<UserDto>> GetAllUsers()
        {
            var result = _userDal.GetUsers();

            if(result.Count > 0)
            {
                return new SuccessDataResult<List<UserDto>>(result, "Succesfully fetched");
            }
            else
            {
                return new ErrorDataResult<List<UserDto>>(result, "An error occured");
            }
        }

        public IResult DeleteUser(int id)
        {
            var user = _userDal.Get(u => u.Id == id && u.Status == true);
            
            if(user != null)
            { 
                user.Status = false;
                _userDal.Delete(user);
                return new SuccessResult("User is succesfully deleted");
            }
            else
            {
                return new ErrorResult("User is not found");
            }
        }
        public IResult ActivateUser(int id)
        {
            var user = _userDal.Get(u => u.Id == id);

            if (user != null)
            {
                user.Status = true;
                _userDal.Delete(user);
                return new SuccessResult("User is succesfully deleted");
            }
            else
            {
                return new ErrorResult("User is not found");
            }
        }

        public IResult FollowUser(int followedUserId)
        {
            int followerId = int.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            // Check if the relationship already exists
            var existingFollow = _userFollowerDal.Get(uf => uf.FollowerId == followerId  && uf.FollowedUserId == followedUserId);
            if (existingFollow != null)
            {
                return new ErrorResult("You are already following this user.");
            }

            // Add the follow relationship
            var follow = new UserFollower
            {
                FollowerId = followerId,
                FollowedUserId = followedUserId,
                CreateDate = DateTime.Now
            };

            _userFollowerDal.Add(follow);
            return new SuccessResult("You are now following the user.");
        }
        public IResult UnfollowUser(int followedUserId)
        {
            int followerId = int.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var follow = _userFollowerDal.Get(uf => uf.FollowerId == followerId && uf.FollowedUserId == followedUserId);
            if (follow == null)
            {
                return new ErrorResult("You are not following this user.");
            }

            _userFollowerDal.Delete(follow);
            return new SuccessResult("You have unfollowed the user.");
        }


    }
}