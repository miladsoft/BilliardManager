using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Billiard.Entities.AuditableEntity;
using Billiard.Entities.Identity;

namespace Billiard.Entities
{
    public class TechnicalToolsCoin : Entity
    {

        [Key]
        public int Id { get; set; }

        public string NameFa { get; set; }

        public string NameEn { get; set; }

        public string Title { get; set; }

        public string Symbol { get; set; }

        public string ChartSymbol { get; set; }

        public IsActive IsActive { get; set; }

        public int CoinRank { get; set; }

    }

    public enum IsActive
    {
        [Display(Name = "فعال")]
        active = 0,

        [Display(Name = "غیرفعال")]
        deactive = 1

    }


}