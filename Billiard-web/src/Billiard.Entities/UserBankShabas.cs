using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Billiard.Entities.AuditableEntity;
using Billiard.Entities.Identity;

namespace Billiard.Entities
{
    public class UserBankShebas : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string ShebaNumber { get; set; }

        public ShebaStatus ShebaStatus { get; set; }

        public string ShebaOwner { get; set; }

        public string Description { get; set; }

    }

    public enum ShebaStatus
    {
        [Display(Name = "تایید شده")]

        accepted = 1,
        [Display(Name = "رد شده")]

        rejected = 2,
        [Display(Name = "حذف شده")]

        Deleted = 3
    }
}