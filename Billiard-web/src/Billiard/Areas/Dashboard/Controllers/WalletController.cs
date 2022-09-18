using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Billiard.Common.GuardToolkit;
using Billiard.Entities;
using Billiard.paapCWallet;
using Billiard.Services;
using Billiard.Services.Identity;
using Billiard.ViewModels;
using Billiard.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using QRCoder;
using System.Drawing.Imaging;

namespace Billiard.Areas.Dashboard.Controllers
{
    [Area(AreaConstants.DashboardArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(Title = "کیف پول ها", UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("کیف پول ها")]

    public class WalletController : Controller
    {
        private readonly IWalletBilliardServices _walletBilliardManager;
        public WalletController(IWalletBilliardServices walletBilliardManager)
        {
            _walletBilliardManager = walletBilliardManager;
        }
        private int DefaultPageSize = 10;

        [BreadCrumb(Title = "لیست کیف پول ها", Order = 1)]
        [DisplayName("لیست کیف پول ها")]

        public IActionResult Index()
        {
            return View();
        }


        [BreadCrumb(Title = "کیف پول تومان", Order = 1)]
        [DisplayName("کیف پول تومان")]

        public IActionResult Toman()
        {
            return View();
        }

        [BreadCrumb(Title = "کیف پول وبمانی", Order = 1)]
        [DisplayName("کیف پول وبمانی")]

        public IActionResult WebMoney()
        {
            return View();
        }


        [BreadCrumb(Title = "کیف پول سناتور", Order = 1)]
        [DisplayName("کیف پول سناتور")]

        public IActionResult Billiard(int page = 1)
        {

            var userId = this.User.Identity.GetUserId<int>();
            var allAccount = _walletBilliardManager.Query(c => c.UserId == userId).OrderBy(c => c.OrderByDescending(z => z.CreatedDateTime)).SelectPage(page, DefaultPageSize, out int TotalItems).ToList();


            var model = new PagedListViewModel<WalletBilliardViewModel>();
            var _list = new List<WalletBilliardViewModel>();
            foreach (var item in allAccount.ToList())
            {
                var Secret = TextEncryptDecrypt.DecryptCipherTextToPlainText(item.Account);
                BilliardService account = new BilliardService(Secret);
                _list.Add(new WalletBilliardViewModel()
                {
                    Address = account.GetAddress(),
                    Amount = account.GetBalance(),
                    Name = item.Name,
                    Id = item.Id
                });
            }
            model.List = _list;
            model.Paging.TotalItems = TotalItems;
            model.Paging.CurrentPage = page;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;
            return View(model);

        }




        [AjaxOnly]
        public IActionResult AddWalletBilliard()
        {
            var userId = this.User.Identity.GetUserId<int>();

            var model = new AddBilliardWalletViewModel();
            Account account = new Account();
            _walletBilliardManager.InsertAsync(new WalletBilliard()
            {
                Account = TextEncryptDecrypt.EncryptPlainTextToCipherText(account.SecretNumber.ToString()),
                Name = "",
                UserId = userId

            });
            model.Address = account.GetAddress();
            model.PubKey = account.GetPubKeyHex();
            model.SecretNumber = account.SecretNumber.ToString();

            return PartialView("_ResultAddBilliardWallet", model);
        }

        [HttpGet]
        public IActionResult GetQRCode(String text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return File(BitmapToBytes(qrCodeImage), "image/jpeg");
        }
        private static Byte[] BitmapToBytes(System.Drawing.Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream,ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
