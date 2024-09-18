using Core.Entities.Abstract;

namespace Entities.DTOs
{
    public class CommentDto : IDto
    {
        public int PostId { get; set; }
        public string CommentText { get; set; }
    }
}
