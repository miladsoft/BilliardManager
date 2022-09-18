using Billiard.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Billiard.Entities.Identity
{

    public class User : IdentityUser<int>, IAuditableEntity
    {
        public User()
        {
            UserUsedPasswords = new HashSet<UserUsedPassword>();
            UserTokens = new HashSet<UserToken>();
        }

        [StringLength(450)]
        public string FirstName { get; set; }

        [StringLength(450)]
        public string LastName { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                var displayName = $"{FirstName} {LastName}";
                return string.IsNullOrWhiteSpace(displayName) ? UserName : displayName;
            }
        }
        public string PhotoFileName { get; set; }

        public string DocPhotoFileName { get; set; }
        public isConfirmed IsDocPhotoConfirmed { get; set; } = isConfirmed.notConfirmed;

        public string SelfiPhotoFileName { get; set; }
        public isConfirmed IsSelfiPhotoConfirmed { get; set; } = isConfirmed.notConfirmed;


        public string HomePhoneNumber { get; set; }
        public string HomePhoneNumberCode { get; set; }
        public isConfirmed IsPhoneNumberConfirmed { get; set; } = isConfirmed.notConfirmed;


        public string MobileNumber { get; set; }
        public isConfirmed IsMobileNumberConfirmed { get; set; } = isConfirmed.notConfirmed;


        public DateTimeOffset? BirthDate { get; set; }
        public isConfirmed IsBirthDateConfirmed { get; set; } = isConfirmed.notConfirmed;

        public DateTime? CreatedDateTime { get; set; }

        public DateTimeOffset? LastVisitDateTime { get; set; }

        public bool IsEmailPublic { get; set; }

        public string Location { set; get; }
        public string LocationCode { set; get; }

        public isConfirmed IsLocationConfirmed { get; set; } = isConfirmed.notConfirmed;


        public string PostalCode { set; get; }
        public isConfirmed IsPostalCodeConfirmed { get; set; } = isConfirmed.notConfirmed;

        public string MelliCode { set; get; }
        public isConfirmed IsMelliCodeConfirmed { get; set; } = isConfirmed.notConfirmed;

        public bool IsActive { get; set; } = true;

        public bool IsBlueTick { get; set; } = false;



        public bool? UserChangeInfo { get; set; }


        public string Job { get; set; }

        public string UserDescription { get; set; }


        public string UserInfoDescription { get; set; }

        public string MobileDescription { get; set; }
        public string CardDescription { get; set; }
        public string ShebaDescription { get; set; }
        public string DocDescription { get; set; }
        public string SelfiDescription { get; set; }
        public string HomePhoneDescription { get; set; }
        public string AddressDescription { get; set; }

        public string YourRefferCode { get; set; }

        public string Referrer { get; set; }




        public virtual ICollection<UserUsedPassword> UserUsedPasswords { get; set; }

        public virtual ICollection<UserToken> UserTokens { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }

        public virtual ICollection<UserLogin> Logins { get; set; }

        public virtual ICollection<UserClaim> Claims { get; set; }

        public virtual ICollection<UserBankCards> UserBankCards { get; set; }

        public virtual ICollection<UserBankShebas> UserBankShebas { get; set; }
        public virtual ICollection<UserLog> UserLogs { get; set; }

        public virtual ICollection<BlogPost> BlogPost { get; set; }

        public virtual ICollection<BlogPostComment> BlogPostComment { get; set; }

        public virtual ICollection<BlogPostLike> BlogPostLike { get; set; }

        public virtual ICollection<BlogPostView> BlogPostView { get; set; }



    }

    public enum isConfirmed
    {
        [Display(Name = "منتظر تایید")]
        waitingforconfirmation = 1,

        [Display(Name = "تایید شده")]
        confirmed = 2,

        [Display(Name = "تایید نشده")]
        notConfirmed = 3
    }
}