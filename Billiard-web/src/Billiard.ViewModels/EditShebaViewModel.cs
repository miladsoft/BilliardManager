using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
using Billiard.Entities;
using System;

namespace Billiard.ViewModels
{
    public class EditShebaViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        [Display(Name = "شماره شبا")]
        [ValidIranShetabNumberAttribute(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]

        public string ShebaNumber { get; set; }
 

        [Display(Name = "وضعیت شبا")]
        public ShebaStatus ShebaStatus { get; set; }

        [Display(Name = "صاحب شبا")]
        public string ShebaOwner { get; set; }
     
        [Display(Name = "توضیحات")]

        public string Description { get; set; }




 
    }
}