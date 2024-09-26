using Business.Abstract;
using Core.Entities.Concrete;
using Core.Helpers.Results.Abstract;
using Core.Helpers.Results.Concrete;
using Core.Helpers.Security.Hashing;
using Core.Helpers.Security.JWT;
using Entities.DTOs;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<User> Register(RegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                Username = userForRegisterDto.Username,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                JoinDate = DateTime.Now,
                UpdateTime = DateTime.Now,
                
                Status = true
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, "Registration is succesfully completed");
        }

        public IDataResult<User> Login(LoginDto userForLoginDto)
        {
            //var userToCheck = _userService.GetByMail(userForLoginDto.Username);
            var userToCheck = _userService.GetByUsername(userForLoginDto.Username);
            
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>("User is not found");
            }

            if (!HashingHelper.VerifiedPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>("Username or password is wrong");
            }

            return new SuccessDataResult<User>(userToCheck, "Succesfully logged in");
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult("this user is already exists");
            }
            return new SuccessResult();
        }
        public IResult UserExistsByUsername(string username)
        {
            if (_userService.GetByUsername(username) != null)
            {
                return new ErrorResult("This user is succesfully exists");
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateAccessToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, "Token is successfully created");
        }
    }
}