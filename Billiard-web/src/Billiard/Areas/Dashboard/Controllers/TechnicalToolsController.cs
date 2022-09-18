using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Billiard.Common.IdentityToolkit;
using Billiard.Services;
using Billiard.Services.Contracts.Identity;
using Billiard.Services.Identity;
using Billiard.ViewModels;
using Billiard.ViewModels.Identity;
using System.Linq;

namespace Billiard.Areas.Dashboard.Controllers
{
    [Area(AreaConstants.DashboardArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(Title = "ابزار تحلیل تکنیکال", UseDefaultRouteUrl = true, Order = 0)]
    public class TechnicalToolsController : Controller
    {
        private readonly IApplicationUserManager _userManager;
        private readonly ITechnicalToolsCoinServices _coinManager;

        public TechnicalToolsController(ITechnicalToolsCoinServices coinManager)
        {
            _coinManager = coinManager;
        }


        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index(string Id)
        {
            var model = _coinManager.Query().Select(coin => new TechnicalToolsCoinViewModel()
            {
                NameFa = coin.NameFa,
                NameEn = coin.NameEn,
                ChartSymbol = coin.ChartSymbol,
                Id = coin.Id,
                CoinRank = coin.CoinRank,
                IsActive = coin.IsActive,
                Symbol = coin.Symbol,
                Title = coin.Title
            }).ToList();

            return View(model);
        }


        [AjaxOnly]
        public async Task<IActionResult> RenderItem([FromBody] ModelIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model == null || model.Id == 0)
            {
                return PartialView("_Create", model: new TechnicalToolsCoinViewModel());
            }
            var coin = _coinManager.Query(c => c.Id == model.Id).Select().FirstOrDefault();
            if (coin == null)
            {
                ModelState.AddModelError("", "coinNotFound");
                return PartialView("_Create", model: new TechnicalToolsCoinViewModel()
                {
                    NameFa = coin.NameFa,
                    NameEn = coin.NameEn,
                    ChartSymbol = coin.ChartSymbol,
                    Id = coin.Id,
                    CoinRank = coin.CoinRank,
                    IsActive = coin.IsActive,
                    Symbol = coin.Symbol,
                    Title = coin.Title
                });
            }
            return PartialView("_Create", model: new TechnicalToolsCoinViewModel());
        }


        [HttpPost]

        public async Task<IActionResult> AddItem(TechnicalToolsCoinViewModel model)
        {
            var files = HttpContext.Request.Form.Files;
            if (ModelState.IsValid)
            {
                var result = await _coinManager.InsertAsync(new Entities.TechnicalToolsCoin()
                {
                    NameFa = model.NameFa,
                    NameEn = model.NameEn,
                    ChartSymbol = model.ChartSymbol,
                    Id = model.Id,
                    CoinRank = model.CoinRank,
                    IsActive = model.IsActive,
                    Symbol = model.Symbol,
                    Title = model.Title
                });
                if (result)
                {
                    return Json(new { success = true });
                }
            }
            return PartialView("_Create", model: model);
        }
    }

}