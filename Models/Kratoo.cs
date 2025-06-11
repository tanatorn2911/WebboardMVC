using System;
using System.Collections.Generic;

namespace WebboardMVC.Models
{
    public partial class Kratoo
    {
        public long Kid { get; set; }
        public string? KratooTopic { get; set; }
        public string? KratooDetails { get; set; }
        public long? CategoryId { get; set; }
        public string? RecordDate { get; set; }
        public long? ViewCount { get; set; }
        public long? ReplyCount { get; set; }
        public string? Username { get; set; }
        public string? UserIp { get; set; }
        public string? IsShow { get; set; }
    }
}
