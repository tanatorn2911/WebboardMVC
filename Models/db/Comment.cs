using System;
using System.Collections.Generic;

namespace WebboardMVC.Models.db
{
    public partial class Comment
    {
        public int Kid { get; set; }
        public int CommentNo { get; set; }
        public string? Description { get; set; }
        public DateTime? ReplyDate { get; set; }
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public bool? IsShow { get; set; }

        public virtual Kratoo KidNavigation { get; set; } = null!;
    }
}
