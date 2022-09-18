using System.Text.RegularExpressions;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DNTCommon.Web.Core;
using Billiard.Common.WebToolkit;
using Billiard.Services;
using Billiard.ViewModels;
using System.Threading.Tasks;
using Billiard.Areas;
using Billiard.Services.Identity;

namespace Billiard.Controllers
{
    [BreadCrumb(Title = "مرکز مشاوره", UseDefaultRouteUrl = true, Order = 0)]
    [Area(AreaConstants.DashboardArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    public class ConsultingManagerController : Controller
    {
        private readonly IBilliardConsultingServices _consultingManager;
        public ConsultingManagerController(IBilliardConsultingServices consultingManager)
        {
            _consultingManager = consultingManager;
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {

            return View();
        }
        [BreadCrumb(Title = "گزارش مرکز مشاوره", Order = 1)]
        public IActionResult Report()
        {

            return View();
        }


        [HttpPost]
        [BreadCrumb(Title = "ارسال درخواست مشاوره", Order = 1)]
        public async Task<IActionResult> RequestConsulting(BilliardConsultingViewModel model)
        {

            if (ModelState.IsValid)
            {


                var result = await _consultingManager.InsertAsync(new Entities.BilliardConsulting()
                {
                    Mobile = model.Mobile,
                    Name = model.Name,
                    Subject = model.Subject,
                    Text = model.Text

                });
                if (result)
                {
                    var Successmodel = new AlertMessageViewModel() { IsRefreshPage = "true", MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "درخواست شما با موفقیت در سیستم درج شد" };
                    return PartialView("_Message", model: Successmodel);
                }
            }
            return View("Index", model);


        }
    }
}