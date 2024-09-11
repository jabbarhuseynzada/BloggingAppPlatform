using AutoMapper;
using Core.DataAccess.Concrete;
using Core.Helpers.Results.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Concrete.EF;

public class EfCommentDal(BloggingAppDbContext context) : BaseRepository<Comment, BloggingAppDbContext>(context), ICommentDal
{

}
