using Core.Entities.Concrete;
using Core.Helpers.Results.Abstract;
using Core.Helpers.Security.JWT;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(RegisterDto userForRegisterDto, string password);
        IDataResult<User> Login(LoginDto userForLoginDto);
        IResult UserExists(string email);
        IDataResult<AccessToken> CreateAccessToken(User user);
    }
}
