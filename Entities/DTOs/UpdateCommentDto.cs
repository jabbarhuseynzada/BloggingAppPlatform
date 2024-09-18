using Core.Entities.Abstract;

namespace Entities.DTOs
{
    public class UpdateCommentDto : IDto
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
    }
}
