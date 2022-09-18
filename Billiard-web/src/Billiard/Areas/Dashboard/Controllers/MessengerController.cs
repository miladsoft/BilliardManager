using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Billiard.Services.Identity;

namespace Billiard.Areas.Dashboard.Controllers
{
    [Area(AreaConstants.DashboardArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(Title = "پیام رسان", UseDefaultRouteUrl = true, Order = 0)]
    public class MessengerController : Controller
    {
        [BreadCrumb(Title = "گروه سناتور", Order = 1)]
        public IActionResult Index() => View();
    }
}