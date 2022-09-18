using System.Text.RegularExpressions;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DNTCommon.Web.Core;
using System;
using DNTPersianUtils.Core;
using MD.PersianDateTime.Core;
using Billiard.Common.WebToolkit;
namespace Billiard.Controllers
{
    [BreadCrumb(Title = "معرفی دوستان", UseDefaultRouteUrl = true, Order = 0)]
    public class RefferController : Controller
    {
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index(string Id)
        {
            var strUserAgent = Request.Headers["User-Agent"].ToString();


            if (MobileDeviceBrowser.IsMobileDeviceBrowser(strUserAgent))
            {
                return Redirect("/Mobile/Register/" + Id);
             }
            else
            {               
                 return Redirect("/Dashboard/Register/" + Id);

            }

        }

    }
}