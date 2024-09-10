using Business.Abstract;
using Core.Helpers.Results.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        [HttpPost("addOperationClaim")]
        public IActionResult AddOperationClaimToUser(int userId, int claimId)
        {
            var result = _userService.AddOperationClaimToUser(userId, claimId);
            if(result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
