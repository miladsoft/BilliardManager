namespace Billiard.ViewModels.Identity.Emails
{
    public class TwoFactorSendCodeViewModel : EmailsBase
    {
        public string Token { set; get; }
    }
}