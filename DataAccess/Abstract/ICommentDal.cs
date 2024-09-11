using Core.DataAccess.Abstract;
using Core.Helpers.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ICommentDal : IBaseRepository<Comment>
    {
    }
}
