﻿namespace WebboardMVC.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
