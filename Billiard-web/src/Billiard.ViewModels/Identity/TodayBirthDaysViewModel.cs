using System.Collections.Generic;
using Billiard.Entities.Identity;

namespace Billiard.ViewModels.Identity
{
    public class TodayBirthDaysViewModel
    {
        public List<User> Users { set; get; }

        public AgeStatViewModel AgeStat { set; get; }
    }
}