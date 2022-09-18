using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;
namespace Billiard.ViewModels
{
        public class Owner
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class Result
    {
        public string sheba { get; set; }
        public List<Owner> owners { get; set; }
        public bool block { get; set; }
        public bool hasWillBeBlock { get; set; }
        public bool hasWithdrawBlock { get; set; }
    }

    public class ShebaInfoViewModel
    {
        public bool hasError { get; set; }
        public int messageId { get; set; }
        public string referenceNumber { get; set; }
        public int errorCode { get; set; }
        public int count { get; set; }
        public string ott { get; set; }
        public Result result { get; set; }
    }
}