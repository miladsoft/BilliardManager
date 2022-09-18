using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
namespace Billiard.ViewModels
{
    public class EditSelfiViewModel
    {
         [HiddenInput]
        public int UserId { set; get; }
 

        [Display(Name = "وضعیت تصویر سلفی")]
        public isConfirmed IsSelfiPhotoConfirmed { get; set; }
   

        [Display(Name = "توضیحات")]
        public string SelfiDescription{ get; set; }
    }
}