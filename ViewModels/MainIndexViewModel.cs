using Microsoft.EntityFrameworkCore;
using WebboardMVC.Models.db;

namespace WebboardMVC.ViewModels
{
    public class MainIndexViewModel
    {
        public IEnumerable<Category> Categorylist { get; set; }
        public IEnumerable<Kratoo> KratooList { get; set; }
        public int? MostPopularCategoryId { get; set; } // ID ของหมวดหมู่ที่มีโพสต์มากที่สุด
    }
}
