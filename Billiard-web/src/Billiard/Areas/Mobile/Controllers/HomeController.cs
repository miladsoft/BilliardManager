using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Billiard.Areas.MobileArea.Controllers
{   
    [Authorize]
    [Area(AreaConstants.MobileArea)]
    [BreadCrumb(Title = "خانه", UseDefaultRouteUrl = true, Order = 0)]
    public class HomeController : Controller
    {
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index() => View();

    }
}