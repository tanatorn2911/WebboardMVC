using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebboardMVC.Models;
using WebboardMVC.Models.db;
using WebboardMVC.ViewModels;

namespace WebboardMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly thaivbWebboardContext _db;

        public HomeController(thaivbWebboardContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // นับจำนวนโพสต์ในแต่ละหมวดหมู่
            var categoryWithPostCounts = _db.Categories
                .Select(c => new
                {
                    Category = c,
                    PostCount = c.Kratoos.Count(k => k.IsShow == true) // นับจำนวนกระทู้ที่แสดงอยู่ในแต่ละหมวดหมู่
                }).ToList();

            // หาหมวดหมู่ที่มีโพสต์มากที่สุด
            var mostPopularCategory = categoryWithPostCounts.OrderByDescending(c => c.PostCount).FirstOrDefault();

            var viewmodel = new MainIndexViewModel()
            {
                Categorylist = _db.Categories.ToList(),
                KratooList = _db.Kratoos
                    .OrderByDescending(r => r.RecordDate)
                    .Take(10)
                    .Include(c => c.Category)
                    .Where(i => i.IsShow == true),
                MostPopularCategoryId = mostPopularCategory?.Category.CategoryId // เก็บ ID ของหมวดหมู่ที่มีโพสต์มากที่สุด
            };

            return View(viewmodel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
