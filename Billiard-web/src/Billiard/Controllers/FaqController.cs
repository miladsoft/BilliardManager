using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DNTCommon.Web.Core;

namespace Billiard.Controllers
{
    [BreadCrumb(Title = "سوالات متداول", UseDefaultRouteUrl = true, Order = 0)]
    public class FaqController : Controller
    {
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {
            return View();
        }

    }
}