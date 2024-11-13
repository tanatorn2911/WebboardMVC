using WebboardMVC.Models.db;

namespace WebboardMVC.ViewModels
{
    public class KratooCommentViewModel
    {
        public Kratoo Kratoo { get; set; }
        public IQueryable<Comment> CommentList { get; set; }
    }
}
