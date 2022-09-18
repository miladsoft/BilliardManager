using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Billiard.ViewModels.Identity
{
    public class RegisterViewModel
    {
 
         [Display(Name = "کد معرف (درصورت وجود معرف وارد نمایید)")]
 
        public string Referrer { get; set; }


        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [StringLength(450)]
        [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
                          ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [StringLength(450)]
        [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
                          ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [Remote("ValidateUsername", "Register",
            AdditionalFields = nameof(Email) + "," + ViewModelConstants.AntiForgeryToken, HttpMethod = "POST")]
        [EmailAddress(ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید.")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 6)]
        [Remote("ValidatePassword", "Register",
            AdditionalFields = nameof(Email) + "," + ViewModelConstants.AntiForgeryToken, HttpMethod = "POST")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه‌ی عبور")]
        
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه‌ی عبور")]
        [Compare(nameof(Password), ErrorMessage = "کلمات عبور وارد شده با هم تطابق ندارند")]
        public string ConfirmPassword { get; set; }
    }
}