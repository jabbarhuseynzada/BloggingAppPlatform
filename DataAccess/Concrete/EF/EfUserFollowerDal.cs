using Core.DataAccess.Concrete;
using Core.Entities.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EF
{
    public class EfUserFollowerDal(BloggingAppDbContext context) : BaseRepository<UserFollower, BloggingAppDbContext>(context), IUserFollowerDal
    {
        public void DeleteFollow(UserFollower userFollower)
        {
            var deletedEntity = context.Entry(userFollower);
            deletedEntity.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            context.SaveChanges();
        }
    }
}
