using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebboardMVC.Models;
using WebboardMVC.ViewModels;

namespace WebboardMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var categoryWithPostCounts = _db.Kratoos
     .Where(k => k.CategoryId != null)
     .GroupBy(k => k.CategoryId)
     .Select(g => new
     {
         CategoryId = g.Key,
         PostCount = g.Count()
     })
     .ToList();

            // หาหมวดหมู่ที่มีโพสต์มากที่สุด
            var mostPopularCategory = categoryWithPostCounts
                .OrderByDescending(c => c.PostCount)
                .FirstOrDefault();

            // สร้าง ViewModel
            var viewmodel = new MainIndexViewModel()
            {
                Categorylist = _db.Categories.Where(k => k.CategoryId != null).ToList(),

                KratooList = _db.Kratoos
                    .Where(i => i.IsShow == "true")
                    .OrderByDescending(r => r.RecordDate)
                    .Take(10)
                    .ToList(),

                MostPopularCategoryId = Convert.ToInt32(mostPopularCategory?.CategoryId ?? 0) 
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
