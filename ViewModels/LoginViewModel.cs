using System.ComponentModel.DataAnnotations;

namespace WebboardMVC.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "กรุณาป้อน Email")]
        [Display(Name = "E-mail : ")]
        public String Email { get; set; }
        [Required(ErrorMessage = "กรุณาป้อน Password")]
        [Display(Name = "Password : ")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}
