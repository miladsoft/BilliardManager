using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Billiard.Entities.AuditableEntity;
using Billiard.Entities.Identity;

namespace Billiard.Entities
{
    public class BilliardConsulting : Entity
    {

        [Key]
        public int Id { get; set; }

        public string Subject { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public string Text { get; set; }


        public int UserId { get; set; }
     
        public string Description { get; set; }

        public DateTimeOffset FollowUpDateTime { get; set; } 


    }
}