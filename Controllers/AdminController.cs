using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebboardMVC.Models;
using WebboardMVC.ViewModels;

namespace WebboardMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Where(u => u.IsActive).ToListAsync();
            var viewmodel = new List<UserRolesViewModel>();
            

            foreach (var item in users)
            {
                var vm = new UserRolesViewModel();
                vm.UserId = item.Id;
                vm.Email = item.Email;
                vm.UserName = item.UserName;
                vm.FullName = item.FullName;
                vm.Roles = await GetRolesList(item);
                viewmodel.Add(vm);
            }

            return View(viewmodel);
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        private async Task<List<string>> GetRolesList(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel data)
        {
            
                ApplicationUser user = new ApplicationUser();
                user.UserName = data.Email;
                user.Email = data.Email;
                user.FullName = "";
                user.Address = "";
                user.ImageUrl = "";

                var result = await _userManager.CreateAsync(user, data.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        var role = new IdentityRole("Admin");
                        var roleresult = await _roleManager.CreateAsync(role);
                    }

                    await _userManager.AddToRoleAsync(user, "Admin");
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            

            return View(data);
        }

        public IActionResult AddRoleToUser(string id)
        {
            ApplicationUser user = _userManager.FindByIdAsync(id).Result;

            var viewmodel = new AddRoleToUserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = GetAllRoles()
            };

            return View(viewmodel);
        }

        private SelectList GetAllRoles()
        {
            return new SelectList(_roleManager.Roles.OrderBy(u => u.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserViewModel data)
        {
            ApplicationUser user = _userManager.FindByIdAsync(data.UserId).Result;

          
                if (!await _userManager.IsInRoleAsync(user, data.NewRole))
                {
                    var result = await _userManager.AddToRoleAsync(user, data.NewRole);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            

            return View(data);
        }

        public async Task<IActionResult> RemoveRoleFromUser(string id)
        {
            ApplicationUser user = _userManager.FindByIdAsync(id).Result;
            var userRoles = string.Join(",", await _userManager.GetRolesAsync(user));

            var viewmodel = new RemoveRoleFromUserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = userRoles
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRoleFromUser(RemoveRoleFromUserViewModel data, string role)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(data.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains(role))
            {
     
                await _userManager.RemoveFromRoleAsync(user, role);

                return RedirectToAction("Index", "Admin");
            }

          
            ModelState.AddModelError("", "ไม่พบสิทธิ์ดังกล่าวในผู้ใช้");
            return View(data);
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
              
                user.IsActive = false;

                var result = await _userManager.UpdateAsync(user); 
                if (result.Succeeded)
                {
                    return RedirectToAction("Index"); 
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction("Index"); 
        }

    }
}
