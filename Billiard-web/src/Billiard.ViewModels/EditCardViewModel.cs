using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
using Billiard.Entities;
using System;

namespace Billiard.ViewModels
{
    public class EditCardViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        [Display(Name = "شماره کارت")]
        [ValidIranShetabNumberAttribute(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        public string CardNumber { get; set; }

        [Display(Name = "نام بانک")]
        public string BankName { get; set; }

        [Display(Name = "وضعیت کارت")]
        public CardStatus CardStatus { get; set; }

        [Display(Name = "صاحب کارت")]
        public string CardOwner { get; set; }
     
        [Display(Name = "توضیحات")]

        public string Description { get; set; }
    }
}