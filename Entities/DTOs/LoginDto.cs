using Core.Entities.Abstract;

namespace Entities.DTOs
{
    public class LoginDto : IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
