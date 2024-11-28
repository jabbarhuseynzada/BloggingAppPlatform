using Core.Entities.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class GetUserDto : IDto
    {
        public User User {  get; set; }
        public int FollowerCount { get; set; }
    }
}
