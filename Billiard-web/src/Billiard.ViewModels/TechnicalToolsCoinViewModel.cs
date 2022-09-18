using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using Billiard.Entities;

namespace Billiard.ViewModels
{
    public class TechnicalToolsCoinViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "نام به فارسی")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [StringLength(450)]
        [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
                          ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
        public string NameFa { get; set; }

        [Display(Name = "نام به انگلیسی")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        public string NameEn { get; set; }
        [Display(Name = "عنوان چارت")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]

        public string Title { get; set; }

        [Display(Name = "نماد")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        public string Symbol { get; set; }

        [Display(Name = "نماد در چارت")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        public string ChartSymbol { get; set; }
 
        [Display(Name = "وضعیت رمزارز")]
        public IsActive IsActive { get; set; }

        [Display(Name = "رتبه رمزارز")]
        public int CoinRank { get; set; }

    }

}