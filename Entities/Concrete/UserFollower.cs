using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class UserFollower : BaseEntity
    {
        public int FollowerId { get; set; }

        // The user being followed
        public int FollowedUserId { get; set; }
    }
}
