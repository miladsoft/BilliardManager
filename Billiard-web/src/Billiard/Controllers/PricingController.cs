using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billiard.Controllers
{
    [BreadCrumb(Title = "کارمزدها", UseDefaultRouteUrl = true, Order = 0)]

    public class PricingController : Controller
    {
        [BreadCrumb(Title = "ایندکس", Order = 1)]

        public IActionResult Index()
        {
            return View();
        }
    }
}
