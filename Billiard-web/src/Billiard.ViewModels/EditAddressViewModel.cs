using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
namespace Billiard.ViewModels
{
    public class EditAddressViewModel
    {
        [HiddenInput]
        public int UserId { set; get; }

        [Display(Name = "آدرس")]
        public string Location { set; get; }

        [Display(Name = "وضعیت آدرس")]
        public isConfirmed IsLocationConfirmed { get; set; } = isConfirmed.notConfirmed;

        [Display(Name = "کد پستی")]  
        public string PostalCode { set; get; }

        [Display(Name = "وضعیت کد پستی")]
        public isConfirmed IsPostalCodeConfirmed { get; set; } = isConfirmed.notConfirmed;

        [Display(Name = "توضیحات")]
        public string AddressDescription{ get; set; }
    }
}