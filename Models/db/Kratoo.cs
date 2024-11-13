using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebboardMVC.Models.db
{
    public partial class Kratoo
    {
        public Kratoo()
        {
            Comments = new HashSet<Comment>();
        }


        public int Kid { get; set; }
        [Required(ErrorMessage ="Please enter ")]
        [Display(Name ="Title")]
        [StringLength(100,MinimumLength=1,ErrorMessage ="Max Digit")]
        public string? KratooTopic { get; set; }
        [Required(ErrorMessage = "Please enter")]
        [Display(Name = "Detail")]
        public string? KratooDetails { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public DateTime? RecordDate { get; set; }
        public int? ViewCount { get; set; }
        public int? ReplyCount { get; set; }
        //[Required(ErrorMessage = "กรุณาป้อนชื่อผู้ตั้งกระทู้")]
        [Display(Name = "Owner")]
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public bool? IsShow { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
