using Billiard.Entities.Identity;

namespace Billiard.ViewModels.Identity.Emails;

public class RegisterEmailConfirmationViewModel : EmailsBase
{
    public User User { set; get; }
    public string EmailConfirmationToken { set; get; }
}