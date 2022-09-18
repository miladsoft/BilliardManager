using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DNTCommon.Web.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Billiard.Entities;

namespace Billiard.ViewModels
{


    public class BlogPostViewModel
    {

        public const string AllowedImages = ".png,.jpg,.jpeg,.gif";


        [HiddenInput]
        public int Id { get; set; }
 


        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }
       
       
       
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [Display(Name = "متن پست")]
        public string Text { get; set; }
       
       
       
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [Display(Name = "توضیح کوتاه")]
        public string SeoDescription { get; set; }
       
       
       
       
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [Display(Name = "کلمات کلیدی")]
        public string SeoKeyWords { get; set; }

        [Display(Name = "آدرس تصویر")]
        public string ImageFileName { get; set; }

        [UploadFileExtensions(AllowedImages,
            ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        [Display(Name = "آدرس ویدیو آپارات")]
        public string  VideoFileName { get; set; }

        public IFormFile Video { get; set; }

        [Required(ErrorMessage = "لطفاً {0} را انتخاب کنید")]
        
        [Display(Name = "دسته بندی")]
        public int BlogPostCategoryId { get; set; }
       
       
        [Display(Name = "پست ویژه")]
        public bool IsSpecial { get; set; } = false;

    }
}