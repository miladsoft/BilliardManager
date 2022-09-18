using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
namespace Billiard.ViewModels
{
    public class EditPhoneViewModel
    {
         [HiddenInput]
        public int UserId { set; get; }


        [Display(Name = "شماره تلفن ثابت")]
        public string PhoneNumber { get; set; }


        [Display(Name = "وضعیت شماره تلفن ثابت")]
        public isConfirmed IsPhoneNumberConfirmed { get; set; }

        

        [Display(Name = "توضیحات")]
        public string HomePhoneDescription{ get; set; }
    }
}