using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.DTOs;

namespace BloggingAppPlatform.MVC.ViewModels
{
    public class PostVM
    {
        public List<GetPostDto> Posts { get; set; }
        public List<GetPostDto> PostsByUser { get; set; }
        public List<GetCommentDto> Comments { get; set; }
        public User user { get; set; }
        public int CommentCount { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool IsFollow { get; set; }
        public int FollowerCount { get; set; }
    }
}
