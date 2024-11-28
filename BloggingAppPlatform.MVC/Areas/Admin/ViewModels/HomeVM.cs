using System.Diagnostics.Contracts;

namespace BloggingAppPlatform.MVC.Areas.Admin.ViewModels
{
    public class HomeVM
    {
        public int UserCount { get; set; }
        public int PostCount { get; set; }
        public int CommentCount { get; set; }
    }
}
