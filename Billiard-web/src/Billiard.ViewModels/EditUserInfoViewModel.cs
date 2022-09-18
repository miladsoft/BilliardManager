using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
namespace Billiard.ViewModels
{
    public class EditUserInfoViewModel
    {
         [HiddenInput]
        public int UserId { set; get; }


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

        [Display(Name = "تاریخ تولد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [ValidPersianDateTime(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        public string BirthDate { get; set; }
      
        [Display(Name = "وضعیت تاریخ تولد")]
        public isConfirmed IsBirthDateConfirmed { get; set; } = isConfirmed.notConfirmed;



        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(450)]
        [ValidIranianNationalCode(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        public string MelliCode { get; set; }

        [Display(Name = "وضعیت کد ملی")]
        public isConfirmed IsMelliCodeConfirmed { get; set; } = isConfirmed.notConfirmed;

        [Display(Name = "توضیحات")]

        public string UserInfoDescription { get; set; }
    }
}