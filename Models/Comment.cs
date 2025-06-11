using System;
using System.Collections.Generic;

namespace WebboardMVC.Models
{
    public partial class Comment
    {
        public long Kid { get; set; }
        public long? CommentNo { get; set; }
        public string? Description { get; set; }
        public string? ReplyDate { get; set; }
        public string? Username { get; set; }
        public string? UserIp { get; set; }
        public string? IsShow { get; set; }
    }
}
