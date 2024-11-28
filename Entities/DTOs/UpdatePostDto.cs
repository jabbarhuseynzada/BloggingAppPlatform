using Core.Entities.Abstract;

namespace Entities.DTOs
{
    public class UpdatePostDto : IDto
    {
        public int PostId { get; set; }
        //public int UserId { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        //public string CoverImageUrl { get; set; }
    }
}
