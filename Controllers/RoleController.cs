using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebboardMVC.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var role = _roleManager.Roles.OrderBy(r => r.Name);

            return View(role);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole data)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync(data.Name))
                {
                    var role = new IdentityRole(data.Name);
                    var roleresult = await _roleManager.CreateAsync(role);

                    if (roleresult.Succeeded)
                    {
                        return RedirectToAction("Index", "Role");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Role");
                }
            }

            return View(data);
        }
    }
}
