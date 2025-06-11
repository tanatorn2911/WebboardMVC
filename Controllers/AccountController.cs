using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebboardMVC.Models;
using WebboardMVC.ViewModels;

namespace WebboardMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManage;
        private readonly SignInManager<AppUser> _signInManage;

        public AccountController(UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole>roleManager,
                                 SignInManager<AppUser> signInManager
                                 ) 
        {
            _userManager = userManager;
            _roleManage = roleManager;
            _signInManage = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel data)
        {
            AppUser user = new AppUser();
            user.UserName = data.Email;
            user.Email = data.Email;

            var result = await _userManager.CreateAsync(user,data.Password); //จะสร้าง IDตอนนี้
            if (result.Succeeded) 
            {
                if (!await _roleManage.RoleExistsAsync("Member"))
                {
                    var role = new IdentityRole("Member");
                    var roleresult= await _roleManage.CreateAsync(role);  
                }
                await _userManager.AddToRoleAsync(user, "Member");
                await _signInManage.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index","Member");
            }
            else 
            {
                foreach (var item in result.Errors) 
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Index", data);
        }

        public IActionResult login() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel data) 
        {
            var result =await _signInManage.PasswordSignInAsync(data.Email, data.Password, isPersistent: false, lockoutOnFailure:false );
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                ModelState.AddModelError("", "loginFail");
            }
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() 
        {
            await _signInManage.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        
    }
}
