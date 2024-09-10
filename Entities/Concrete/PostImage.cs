using Core.Entities.Abstract;
using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class PostImage : IEntity
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string ImgUrl { get; set; }
        public bool IsFeatured { get; set; } = true;
    }
}