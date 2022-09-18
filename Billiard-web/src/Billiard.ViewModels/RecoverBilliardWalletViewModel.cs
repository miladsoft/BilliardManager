using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;
namespace Billiard.ViewModels
{
    public class RecoverBilliardWalletViewModel
    {

         [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [Display(Name = "رمز خصوصی")]
        public string SecretNumber { get; set; }



    }
}