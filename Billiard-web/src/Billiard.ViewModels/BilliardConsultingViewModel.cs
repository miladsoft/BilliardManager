using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;
using Billiard.Entities.AuditableEntity;
using Billiard.Entities.Identity;

namespace Billiard.ViewModels
{
    public class BilliardConsultingViewModel  
    {

        [Key]
        public int Id { get; set; }
        [Display(Name = "موضوع")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        public string Subject { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]

        public string Name { get; set; }

        [Display(Name = "شماره موبایل")]
        [ValidIranianMobileNumber(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]

        public string Mobile { get; set; }
        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]

        public string Text { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }
       
        [Display(Name = "تاریخ پیگیری")]
        public DateTimeOffset FollowUpDateTime { get; set; }


    }
}