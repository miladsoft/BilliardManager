using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Billiard.Services.Identity;

namespace Billiard.Areas.Dashboard.Controllers
{
    [Area(AreaConstants.DashboardArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(Title = "صرافی", UseDefaultRouteUrl = true, Order = 0)]
    public class SwapController : Controller
    {
        [BreadCrumb(Title = "مبادله رمزارز", Order = 1)]
        public IActionResult Index() => View();

 
        
    }
}