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
using System.Security.Policy;
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
        public IResult AddOperationClaimToUser(string username, string operationClaimName)
        {
            var userExists = _userDal.Get(u => u.Username == username);
            if (userExists == null)
            {
                return new ErrorResult("User does not exist.");
            }

            var operationClaimExists = _operationClaimDal.Get(c => c.Name == operationClaimName);
            if (operationClaimExists == null)
            {
                return new ErrorResult("Operation claim does not exist.");
            }

            var claim = new UserOperationClaim()
            {
                UserId = userExists.Id,
                OperationClaimId = operationClaimExists.Id
            };

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
        public IDataResult<List<OperationClaim>> GetAllOperationClaims()
        {
            var result = _operationClaimDal.GetAll();
            if(result != null)
            {
                return new SuccessDataResult<List<OperationClaim>>(result, "Operation claims succesfully fetched");
            }
            else
            {
                return new ErrorDataResult<List<OperationClaim>>(result, "There is no operation claims in database");
            }
        }
        public GetUserDto GetUserById(int userId)
        {
            GetUserDto userDto = new()
            {
                User = _userDal.Get(u => u.Id == userId),
                FollowerCount = _userFollowerDal.GetAll(u => u.FollowedUserId == userId).Count(),
            };
            return userDto;
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

        public IResult FollowUser(int followerId, int followedUserId)
        {
            var existingFollow = _userFollowerDal.GetAll(uf => uf.FollowerId == followerId  && uf.FollowedUserId == followedUserId && uf.IsDeleted == false);
            if (existingFollow.Count >= 1)
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
        public IResult UnfollowUser(int followerId,  int followedUserId)
        {
            //int followerId = int.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var existingFollow = _userFollowerDal.GetAll(uf => uf.FollowerId == followerId && uf.FollowedUserId == followedUserId && uf.IsDeleted==false);
            if (existingFollow.Count < 1 )
            {
                return new ErrorResult("You are not following this user.");
            }
            else
            {
                var follow = _userFollowerDal.Get(uf => uf.FollowerId == followerId && uf.FollowedUserId == followedUserId);
                _userFollowerDal.DeleteFollow(follow);
                return new SuccessResult("You have unfollowed the user.");
            }
        }
        public IDataResult<bool> IsFollow(int followerId, int followedUserId)
        {
            var existingFollow = _userFollowerDal.GetAll(uf => uf.FollowerId == followerId && uf.FollowedUserId == followedUserId && uf.IsDeleted == false);
            if (existingFollow.Count < 1)
            {
                return new ErrorDataResult<bool>(false,"You are not following this user.");
            }
            else
            {
                return new SuccessDataResult<bool>(true, "You follow this user.");
            }
        }

        public IResult UpdateUser(UpdateUserDto userDto, int userId)
        {
            User uUser = _userDal.Get(u => u.Id == userDto.Id && u.Status == true);

            if (uUser != null && uUser.Id == userId)
            {
                bool emailOrUsernameExists = _userDal.GetAll(u =>
                    (u.Email == userDto.Email || u.Username == userDto.Username) && u.Id != userId
                ).Any();

                if (emailOrUsernameExists)
                {
                    return new ErrorResult("This email or username is already owned by another user.");
                }
                uUser.Username = userDto.Username;
                uUser.Email = userDto.Email;
                uUser.UpdateTime = DateTime.Now;
                uUser.FirstName = userDto.Firstname;
                uUser.LastName = userDto.Lastname;
                _userDal.Update(uUser);
                return new SuccessResult("User is successfully updated.");
            }
            else
            {
                return new ErrorResult("You cannot update the user.");
            }
        }

    }
}