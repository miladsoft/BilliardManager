using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
namespace Billiard.ViewModels
{
    public class EditDocViewModel
    {
         [HiddenInput]
        public int UserId { set; get; }
 

        [Display(Name = "وضعیت مدرک شناسایی")]
        public isConfirmed IsDocPhotoConfirmed { get; set; }
   

        [Display(Name = "توضیحات")]
        public string DocDescription{ get; set; }
    }
}