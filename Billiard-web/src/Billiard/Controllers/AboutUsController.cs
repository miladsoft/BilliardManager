using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DNTCommon.Web.Core;

namespace Billiard.Controllers
{
    [BreadCrumb(Title = "درباره ما", UseDefaultRouteUrl = true, Order = 0)]
    public class AboutUsController : Controller
    {
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {
            return View();
        }

    }
}