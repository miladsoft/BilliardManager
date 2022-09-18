using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;
namespace Billiard.ViewModels
{
    public class CardInfoViewModel
    {
        public bool hasError { get; set; }
        public int messageId { get; set; }
        public string referenceNumber { get; set; }
        public int errorCode { get; set; }
        public int count { get; set; }
        public string ott { get; set; }
        public string result { get; set; }

    }
}