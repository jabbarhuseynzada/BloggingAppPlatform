using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class GetCommentDto : IDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public string Username { get; set; }
        public string CommentText { get; set; } 
        public DateTime CommentTime { get; set; } 
    }
}
