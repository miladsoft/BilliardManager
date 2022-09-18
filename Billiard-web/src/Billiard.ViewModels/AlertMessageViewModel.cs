using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
namespace Billiard.ViewModels
{
    public class AlertMessageViewModel
    {
        public AlertMessageType MessageType { get; set; }
        public string MessageText { get; set; }
        public string MessageTitle { get; set; }
        
        public string  IsRefreshPage { get; set; }


    }
    public enum AlertMessageType
    {
        success = 1,
        warning = 2,
        danger = 3
    }

}