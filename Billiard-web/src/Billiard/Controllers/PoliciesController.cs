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
    [BreadCrumb(Title = "سناتور", UseDefaultRouteUrl = true, Order = 0)]
    public class PoliciesController : Controller
    {
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {


            var strUserAgent = Request.Headers["User-Agent"].ToString();


            if (MobileDeviceBrowser.IsMobileDeviceBrowser(strUserAgent))
            {
                Response.Redirect("/Mobile");
            }

            return View();
        }

        [BreadCrumb(Title = "ارجاع کاربران", Order = 1)]
        public IActionResult Referral()
        {
            return View();
        }

        [BreadCrumb(Title = "سطوح کاربران", Order = 1)]
        public IActionResult UserLevels()
        {
            return View();
        }

        [BreadCrumb(Title = "خطا", Order = 1)]
        public IActionResult Error()
        {
            return View();
        }


        [Authorize]
        public IActionResult CallBackResult(long token, string status, string orderId, string terminalNo, string rrn)
        {
            var userId = User.Identity.GetUserId();
            return Json(new { userId, token, status, orderId, terminalNo, rrn });
        }
    }
}