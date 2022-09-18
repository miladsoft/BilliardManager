using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Billiard.Entities.AuditableEntity;
using Billiard.Entities.Identity;

namespace Billiard.Entities
{
    public class UserBankCards : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string CardNumber { get; set; }

        public string BankName { get; set; }

        public CardStatus CardStatus { get; set; }

        public string CardOwner { get; set; }

        public string Description { get; set; }


    }

    public enum CardStatus
    {
        [Display(Name = "تایید شده")]
        accepted = 1,
       
    
        [Display(Name = "رد شده")]
        rejected = 2,
       
       
        [Display(Name = "حذف شده")]
        Deleted = 3
    }
}