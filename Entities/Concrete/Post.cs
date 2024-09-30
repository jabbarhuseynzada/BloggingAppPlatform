using Entities.Abstract;

namespace Entities.Concrete;

public class Post : BaseEntity
{
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Context { get; set; }
    //public string CoverImageUrl { get; set; }
    public DateTime? UpdateTime { get; set; }
}