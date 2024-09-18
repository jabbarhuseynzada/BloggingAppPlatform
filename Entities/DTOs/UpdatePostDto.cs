using Core.Entities.Abstract;

namespace Entities.DTOs
{
    public class UpdatePostDto : IDto
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostContext { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
