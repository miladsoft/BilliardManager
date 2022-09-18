using Billiard.Entities.Identity;

namespace Billiard.ViewModels.Identity.Emails
{
    public class UserProfileUpdateNotificationViewModel : EmailsBase
    {
        public User User { set; get; }
    }
}