using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
namespace Billiard.ViewModels
{
    public class EditMobileViewModel
    {
         [HiddenInput]
        public int UserId { set; get; }


        [Display(Name = "شماره موبایل")]
        [ValidIranianMobileNumber(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        public string MobileNumber { get; set; }


        [Display(Name = "وضعیت شماره موبایل")]
        public isConfirmed IsMobileNumberConfirmed { get; set; }

        

        [Display(Name = "توضیحات")]
        public string MobileDescription{ get; set; }
    }
}