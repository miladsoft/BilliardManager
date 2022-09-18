using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DNTCommon.Web.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Billiard.Entities;

namespace Billiard.ViewModels
{      
     

    public class BlogPostCategoryViewModel
    { 
        
        public const string AllowedImages = ".png,.jpg,.jpeg,.gif";
        
        
        [HiddenInput]
        public int Id { get; set; }


        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [Display(Name = "نام انگلیسی")]
          [RegularExpression("^[a-zA-Z_]*$", ErrorMessage = "لطفا تنها از حروف انگلیسی استفاده نمائید")]
        public string Name { get; set; }



        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }


        [Display(Name = "آدرس تصویر")]
        public string ImageFileName { get; set; }

        [UploadFileExtensions(AllowedImages,
            ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
        
        public virtual ICollection<BlogPost> BlogPosts { get; set; }

    }
}