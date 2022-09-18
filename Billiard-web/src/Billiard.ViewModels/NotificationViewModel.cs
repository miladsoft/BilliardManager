using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DNTPersianUtils.Core;
using Billiard.Entities.Identity;
using Billiard.Entities;
using System;

namespace Billiard.ViewModels
{
    public class NotificationViewModel
    {       
         public string Type { get; set; }
         public int Id { get; set; }
        public string Message { get; set; }
    }
}