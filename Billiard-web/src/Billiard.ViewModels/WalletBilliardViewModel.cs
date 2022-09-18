using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
using System;

namespace Billiard.ViewModels
{
    public class WalletBilliardViewModel
    {
        [HiddenInput]
        public Guid Id { set; get; }


        [Display(Name = "آدرس")]
        public string Address { get; set; }
        [Display(Name = "نام کیف پول")]
        public string Name { get; set; }
        [Display(Name = "موجودی")]
        public double Amount { get; set; }
        public string ImageFileName { get; set; }

        public bool IsDelete { get; set; }



    }
}