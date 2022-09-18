using Billiard.Entities.Identity;
using DNTPersianUtils.Core;

namespace Billiard.ViewModels.Identity
{
    public class AgeStatViewModel
    {
        public const char RleChar = (char)0x202B;

        public int UsersCount { set; get; }
        public int AverageAge { set; get; }
        public User MaxAgeUser { set; get; }
        public User MinAgeUser { set; get; }

        public string MinMax => $"{RleChar}جوان‌ترین عضو: {MinAgeUser.DisplayName} ({MinAgeUser.BirthDate.Value.GetAge()})، مage‌ترین عضو: {MaxAgeUser.DisplayName} ({MaxAgeUser.BirthDate.Value.GetAge()})، در بین {UsersCount} نفر";
    }
}