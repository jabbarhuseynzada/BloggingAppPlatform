﻿using Business.Abstract;
using Core.Entities.Concrete;
using Core.Helpers.Results.Abstract;
using Core.Helpers.Results.Concrete;
using DataAccess.Abstract;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IUserOperationClaimDal _userOperationClaimDal;

        public UserManager(IUserDal userDal, IUserOperationClaimDal userOperationClaimDal)
        {
            _userDal = userDal;
            _userOperationClaimDal = userOperationClaimDal;
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
        public IResult AddOperationClaimToUser(int userId, int operationClaimId)
        {
            var claim = new UserOperationClaim()
            {
                UserId = userId,
                OperationClaimId = operationClaimId
            };
            var checkClaim = _userOperationClaimDal.Get(c => c.UserId == claim.UserId && c.OperationClaimId == claim.OperationClaimId);
            if (checkClaim == null)
            {
                _userOperationClaimDal.Add(claim);
                return new SuccessResult("New operation claim succesfuly added to user");
            }
            else return new ErrorResult("This user already acquired this operation claim");
        }
        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }

    }
}