using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;
namespace Billiard.ViewModels
{
    public class AddBilliardWalletViewModel
    {

        public string Address { get; set; }
        public string PubKey { get; set; }
        public string SecretNumber { get; set; }



    }
}