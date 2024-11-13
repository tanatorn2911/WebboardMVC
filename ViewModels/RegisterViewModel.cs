using System.ComponentModel.DataAnnotations;

namespace WebboardMVC.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="กรุณาป้อน Email")]
        [Display(Name ="E-mail : ")]
        public String Email { get; set; }
        [Required(ErrorMessage = "กรุณาป้อน Password")]
        [Display(Name = "Password : ")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [Required(ErrorMessage = "กรุณาป้อน Password")]
        [Display(Name = "Password : ")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="รหัสไม่ตรงกัน")]
        public String PasswordAgain { get; set; }
    }
}
