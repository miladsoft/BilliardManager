using Billiard.Entities.Identity;

namespace Billiard.ViewModels.Identity.Emails;

public class ChangePasswordNotificationViewModel : EmailsBase
{
    public User User { set; get; }
}