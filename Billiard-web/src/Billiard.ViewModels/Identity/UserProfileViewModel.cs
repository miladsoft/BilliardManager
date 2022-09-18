using System.ComponentModel.DataAnnotations;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
using System.Collections.Generic;
using Billiard.Entities;
namespace Billiard.ViewModels.Identity
{
    public class UserProfileViewModel
    {
        public const string AllowedImages = ".png,.jpg,.jpeg,.gif";

        public int Id { get; set; }


        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(450)]
        [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
                          ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(450)]
        [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
                          ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Remote("ValidateUsername", "UserProfile",
            AdditionalFields = nameof(Email) + "," + ViewModelConstants.AntiForgeryToken + "," + nameof(Pid),
            HttpMethod = "POST")]
        [EmailAddress(ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید.")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }



        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(450)]
        [ValidIranianNationalCode(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        public string MelliCode { get; set; }
        public isConfirmed IsMelliCodeConfirmed { get; set; } = isConfirmed.notConfirmed;




        [Display(Name = "تاریخ تولد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [ValidPersianDateTime(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        public string BirthDate { get; set; }
        public isConfirmed IsBirthDateConfirmed { get; set; } = isConfirmed.notConfirmed;


        [Display(Name = "تصویر")]
        [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس تصویر 1000 حرف است.")]
        public string PhotoFileName { set; get; }



        [UploadFileExtensions(AllowedImages,
            ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }



        [UploadFileExtensions(AllowedImages,
            ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
        [DataType(DataType.Upload)]

        public string DocPhotoFileName { get; set; }
        public isConfirmed IsDocPhotoConfirmed { get; set; } = isConfirmed.notConfirmed;


        [UploadFileExtensions(AllowedImages,
            ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
        [DataType(DataType.Upload)]
        public string SelfiPhotoFileName { get; set; }
        public isConfirmed IsSelfiPhotoConfirmed { get; set; } = isConfirmed.notConfirmed;




        [Display(Name = "شماره تلفن ثابت")]
        [ValidIranianPhoneNumber(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        public string PhoneNumber { get; set; }

        [Display(Name = "کد فعالسازی تلفن ثابت")]
        public string PhoneConfirmedCode { set; get; }


        [Display(Name = "شماره موبایل")]
        [ValidIranianMobileNumber(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        public string MobileNumber { get; set; }

        [Display(Name = "کد فعالسازی مویایل")]
        public string MobileConfirmedCode { set; get; }

        [Display(Name = "وضعیت شماره موبایل")]
        public isConfirmed IsMobileNumberConfirmed { get; set; }


        [Display(Name = "کد پستی")]
        [ValidIranianPostalCode(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        public string PostalCode { set; get; }

    
    
    
        [Display(Name = "شماره کارت")]
        public string CardNumber { get; set; }
       
       
       
        [Display(Name = "لیست شماره کارت")]
        public List<UserBankCards> CardNumbers { get; set; }



        [Display(Name = "شماره شبا")]
        [ValidIranShebaNumber(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]

        public string ShebaNumber { get; set; }
       
       
       
        [Display(Name = "لیست شماره شبا")]
        public List<UserBankShebas> ShebaNumbers { get; set; }


        [Display(Name = "آدرس")]
        public string Location { set; get; }
        public isConfirmed IsLocationConfirmed { get; set; } = isConfirmed.notConfirmed;


        [Display(Name = "نمایش عمومی ایمیل")]
        public bool IsEmailPublic { set; get; }

        [Display(Name = "فعال‌سازی اعتبار سنجی دو مرحله‌ای")]
        public bool TwoFactorEnabled { set; get; }

        public bool IsPasswordTooOld { set; get; }

        [HiddenInput]
        public string Pid { set; get; }

        [HiddenInput]
        public bool IsAdminEdit { set; get; }


        [Display(Name = "توضیحات")]

        public string UserInfoDescription { get; set; }
        [Display(Name = "توضیحات")]

        public string MobileDescription { get; set; }
        [Display(Name = "توضیحات")]

        public string CardDescription { get; set; }
        [Display(Name = "توضیحات")]

        public string ShebaDescription { get; set; }
        [Display(Name = "توضیحات")]

        public string DocDescription { get; set; }
        [Display(Name = "توضیحات")]

        public string SelfiDescription { get; set; }
        [Display(Name = "توضیحات")]

        public string HomePhoneDescription { get; set; }
        [Display(Name = "توضیحات")]

        public string AddressDescription { get; set; }
        public string AddressConfirmedCode { get; set; }


        public isConfirmed IsPostalCodeConfirmed { get; set; } = isConfirmed.notConfirmed;

        public isConfirmed IsPhoneNumberConfirmed { get; set; } = isConfirmed.notConfirmed;

    }
}