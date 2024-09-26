using Core.DataAccess.Concrete;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.DTOs;

namespace DataAccess.Concrete.EF;

public class EfUserDal(BloggingAppDbContext context) : BaseRepository<User, BloggingAppDbContext>(context), IUserDal
{
    public List<OperationClaim> GetClaims(User user) //User user   where UOC.UserId == user.Id
    {
        //UserOperations = UserOperationClaims
        using var context = new BloggingAppDbContext();
        var result = from OC in context.OperationClaims
                     join UOC in context.UserOperations
                     on OC.Id equals UOC.OperationClaimId
                     where UOC.UserId == user.Id
                     select new OperationClaim
                     {
                         Id = OC.Id,
                         Name = OC.Name
                     };
        return result.ToList();
    }

    public List<UserDto> GetUsers()
    {
        using var context = new BloggingAppDbContext();
        var result = from U in context.Users
                     where U.Status == true
                     join UOC in context.UserOperations
                     on U.Id equals UOC.UserId
                     join OC in context.OperationClaims
                     on UOC.OperationClaimId equals OC.Id
                     group new { U, OC } by new { U.Id, U.FirstName, U.LastName, U.Email } into grouped
                     select new UserDto
                     {
                         Id = grouped.Key.Id,
                         FirstName = grouped.Key.FirstName,
                         LastName = grouped.Key.LastName,
                         Email = grouped.Key.Email,
                         UserRole = string.Join(", ", grouped.Select(g => g.OC.Name))
                     };
        return result.ToList();
    }
}
