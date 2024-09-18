using Core.Entities.Abstract;

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