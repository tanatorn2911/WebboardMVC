﻿using Microsoft.AspNetCore.Identity;

namespace WebboardMVC.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string? FullName {  get; set; }
        public string? Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
