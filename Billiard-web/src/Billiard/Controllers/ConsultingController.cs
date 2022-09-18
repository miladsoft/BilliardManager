using System.Text.RegularExpressions;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DNTCommon.Web.Core;
using Billiard.Common.WebToolkit;
using Billiard.Services;
using Billiard.ViewModels;
using System.Threading.Tasks;
using Billiard.Entities;
using DNTPersianUtils.Core;

namespace Billiard.Controllers
{
    [BreadCrumb(Title = "مرکز مشاوره", UseDefaultRouteUrl = true, Order = 0)]
    public class ConsultingController : Controller
    {
        private readonly IBilliardConsultingServices _consultingManager;
        public ConsultingController(IBilliardConsultingServices consultingManager)
        {
            _consultingManager = consultingManager;
        }

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


        [HttpPost]
        [BreadCrumb(Title = "ارسال درخواست مشاوره", Order = 1)]
        public async Task<IActionResult> RequestConsulting(BilliardConsultingViewModel model)
        {
 
            if (!model.Mobile.IsValidIranianMobileNumber())
            {
            var Errormobile = new AlertMessageViewModel() { IsRefreshPage = "false", MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد. لطفا شماره موبایل را به درستی وارد نمایید" };
            return PartialView("_Message", model: Errormobile);   
                     }

            if (ModelState.IsValid)
            {
                var result = await _consultingManager.InsertAsync(new BilliardConsulting()
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
            var Errormodel = new AlertMessageViewModel() { IsRefreshPage = "false", MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد. لطفا پارامتر های ورودی را بررسی نمایید" };
            return PartialView("_Message", model: Errormodel);
        }
    }
}