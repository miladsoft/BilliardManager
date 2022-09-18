using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Billiard.Services.Identity;

namespace Billiard.Areas.Dashboard.Controllers
{
    [Area(AreaConstants.DashboardArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(Title = "شبکه اجتماعی", UseDefaultRouteUrl = true, Order = 0)]
    public class SocialController : Controller
    {
        [BreadCrumb(Title = "صفحه شخصی شما", Order = 1)]
        public IActionResult Index()
        {
             return View();

        }

        [BreadCrumb(Title = "جستجو", Order = 1)]
        public IActionResult Search()
        {
            return View();
        }

        [BreadCrumb(Title = "کاوش", Order = 1)]
        public IActionResult Explore()
        {
            return View();
        }

        [BreadCrumb(Title = "مطالب دوستان", Order = 1)]
        public IActionResult Home()
        {
            return View();
        }

        [BreadCrumb(Title = "دنبال کنندگان", Order = 1)]
        public IActionResult Followers()
        {
            return View();
        }

        [BreadCrumb(Title = "دنبال شوندگان", Order = 1)]
        public IActionResult Following()
        {
            return View();
        }
    }
}