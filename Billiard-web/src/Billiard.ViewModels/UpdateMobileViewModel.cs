using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;
namespace Billiard.ViewModels
{
    public class UpdateMobileViewModel
    {
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [Display(Name = "شماره موبایل")]
        [EmailAddress(ErrorMessage = "لطفا شماره موبایل معتبری را وارد نمائید.")]
        [ValidIranianMobileNumber(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]

        public string Mobile { get; set; }

    }
}