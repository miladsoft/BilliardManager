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
    public class HomeController : Controller
    {
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {


            // var strUserAgent = Request.Headers["User-Agent"].ToString();


            // if (MobileDeviceBrowser.IsMobileDeviceBrowser(strUserAgent))
            // {
            //     Response.Redirect("/Mobile");
            // }

            return View();
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index1()
        {
            return View();
        }


        [BreadCrumb(Title = "خطا", Order = 1)]
        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// To test automatic challenge after redirecting from another site
        /// Sample URL: http://localhost:5000/Home/CallBackResult?token=1&status=2&orderId=3&terminalNo=4&rrn=5
        /// </summary>
        [Authorize]
        public IActionResult CallBackResult(long token, string status, string orderId, string terminalNo, string rrn)
        {
            var userId = User.Identity.GetUserId();
            return Json(new { userId, token, status, orderId, terminalNo, rrn });
        }
    }
}