using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DNTCommon.Web.Core;

namespace Billiard.Controllers
{
    [BreadCrumb(Title = "راهنما", UseDefaultRouteUrl = true, Order = 0)]
    public class GuideController : Controller
    {
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {
            return View();
        }

    }
}