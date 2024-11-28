using Entities.DTOs;

namespace BloggingAppPlatform.MVC.Areas.Admin.ViewModels
{
    public class CommentVM
    {
        public List<GetCommentDto> Comments { get; set; }
    }
}
