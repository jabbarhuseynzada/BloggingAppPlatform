using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class CommentDto : IDto
    {
        public int PostId { get; set; }
        public string CommentText { get; set; }
    }
}
