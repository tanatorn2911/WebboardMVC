using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using WebboardMVC.Models;

namespace WebboardMVC.Controllers
{
    [Authorize(Roles = "Member,Admin")]
    public class MemberController : Controller
    {
        private readonly AppDbContext _applicationDbContext;
        private readonly UserManager<AppUser> _userManager;

        public MemberController(AppDbContext applicationDbContext,
            UserManager<AppUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated) 
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                AppUser user = _userManager.FindByIdAsync(userId).Result;
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            else
            {
                return RedirectToAction("Index","Account");
            }
            return View();
        }
        public IActionResult Edit(string id) {
            var userId= _userManager.GetUserId(HttpContext.User);
            AppUser user =_userManager.FindByIdAsync(userId).Result;
            if (user == null) { 
            return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AppUser data, IFormFile files)
        {
            if (id != data.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userStore = new UserStore<AppUser>(_applicationDbContext);
                var userId = _userManager.GetUserId(HttpContext.User);
                AppUser user = _userManager.FindByIdAsync(userId).Result;

                if (user != null)
                {
                    if (files != null)
                    {
                        if (files.Length > 0)
                        {
                            var fileName = Path.GetFileName(files.FileName);
                            var fileExt = Path.GetExtension(fileName);

                            var tmpName = Guid.NewGuid().ToString();
                            var newFileName = string.Concat(tmpName, fileExt);
                            var filePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filePath))
                            {
                                files.CopyTo(fs);
                                fs.Flush();
                            }

                            user.ImageUrl = newFileName.Trim();
                        }
                    }

                    user.FullName = data.FullName;
                    user.Address = data.Address;
                    user.BirthDate = data.BirthDate;
                    user.PhoneNumber = data.PhoneNumber;
                }

                var result = await _userManager.UpdateAsync(user);
                var ctx = userStore.Context;
                await ctx.SaveChangesAsync();

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Member");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(data);
        }
    }
}
