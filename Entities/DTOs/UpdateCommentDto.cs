using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class UpdateCommentDto : IDto
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
    }
}
