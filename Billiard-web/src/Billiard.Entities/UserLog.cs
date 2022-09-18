using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Billiard.Entities;
using Billiard.Entities.Identity;

namespace Billiard.Entities
{
    public class UserLog : Entity
    {
        [Key]
        public virtual int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public string UserAgent { get; set; }
        public string UserIp { get; set; }

        public string UserActivity  { get; set; }

        public DateTimeOffset? DateTimeActivity { get; set; }


    }
}
