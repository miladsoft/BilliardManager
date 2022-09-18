using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Billiard.Entities.AuditableEntity;
using Billiard.Entities.Identity;

namespace Billiard.Entities
{
    public class BlogPostView : Entity
    {
        [Key]
        public int Id { get; set; }

 
        public virtual User User { get; set; }
        public int UserId { get; set; }
        [ForeignKey("BlogPostId")]

        public  virtual BlogPost BlogPost { get; set; }
        [Required]
        public int BlogPostId { get; set; }
        public string UserAgent { get; set; }
        public string UserIp { get; set; }
    }
}