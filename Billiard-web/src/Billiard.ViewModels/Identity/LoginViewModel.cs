using System.ComponentModel.DataAnnotations;

namespace Billiard.ViewModels.Identity
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید.")]

        public string Email { get; set; }

        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [Display(Name = "کلمه‌ی عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "به‌خاطر سپاری کلمه‌ی عبور؟")]
        public bool RememberMe { get; set; }
    }
}