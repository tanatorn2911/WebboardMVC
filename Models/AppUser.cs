using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebboardMVC.Models
{
    public partial class AppUser : IdentityUser
    {
        public long ApplicationUserId { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? BirthDate { get; set; }
        public string? ImageUrl { get; set; }
        public string? IsActive { get; set; }
    }
}
