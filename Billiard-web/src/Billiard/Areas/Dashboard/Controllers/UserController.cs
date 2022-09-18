using System;
using System.Threading.Tasks;
using Billiard.Services.Contracts.Identity;
using Billiard.Services.Identity;
using Billiard.ViewModels.Identity;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Billiard.ViewModels;
using DNTPersianUtils.Core;
using MD.PersianDateTime.Core;
using Billiard.Entities.Identity;
using Billiard.Services;
using System.Linq;

namespace Billiard.Areas.Dashboard.Controllers
{
    [AllowAnonymous]
    [Area(AreaConstants.DashboardArea)]
    [BreadCrumb(Title = "صفحه شخصی", UseDefaultRouteUrl = true, Order = 0)]
    public class UserController : Controller
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IUserBandCardServices _userBandCardServices;
        private readonly IUserBandShebaServices _userBandShebaServices;




        public UserController(
            IApplicationUserManager userManager,
            IApplicationRoleManager roleManager,
            IUserBandCardServices userBandCardServices,
            IUserBandShebaServices userBandShebaServices)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userBandCardServices = userBandCardServices ?? throw new ArgumentNullException(nameof(userBandCardServices));
            _userBandShebaServices = userBandShebaServices ?? throw new ArgumentNullException(nameof(userBandShebaServices));
        }
        [BreadCrumb(Title = "نمایش اطلاعات", Order = 1)]
        public async Task<IActionResult> Index(int? id)
        {


            // var currentUser = _userManager.GetCurrentUser();

            // currentUser.Job = "بنیانگذار سناتور";
            // currentUser.UserDescription = "برنامه نویس فعال در حوزه بیگ دیتا و بلاکچین";

            // var result = await _userManager.UpdateAsync(currentUser);




            if (!id.HasValue && User.Identity.IsAuthenticated)
            {
                id = User.Identity.GetUserId<int>();
            }

            if (!id.HasValue)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdIncludeUserRolesAsync(id.Value);
            if (user == null)
            {
                return View("NotFound");
            }

            var model = new UserItemViewModel
            {
                User = user,
                ShowAdminParts = User.IsInRole(ConstantRoles.Admin),
                Roles = await _roleManager.GetAllCustomRolesAsync(),
                ActiveTab = UserItemActiveTab.UserInfo
            };
            return View(model);
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> EmailToImage(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var fileContents = await _userManager.GetEmailImageAsync(id);
            return new FileContentResult(fileContents, "image/png");
        }

        [BreadCrumb(Title = "لیست کاربران آنلاین", Order = 1)]
        public IActionResult OnlineUsers()
        {
            return View();
        }




        // auth Userinfo
        [AjaxOnly]
        public async Task<IActionResult> RenderEditUserInfo([FromBody] ModelIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                ModelState.AddModelError("", "کاربر یافت نشد");
                return PartialView("_EditUserInfo", model: new EditUserInfoViewModel());
            }
            var _BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value.LocalDateTime.ToPersianYearMonthDay().ToString() : "";

            return PartialView("_EditUserInfo", model: new EditUserInfoViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MelliCode = user.MelliCode,
                BirthDate = _BirthDate,
                IsMelliCodeConfirmed = user.IsMelliCodeConfirmed,
                IsBirthDateConfirmed = user.IsBirthDateConfirmed,
                UserInfoDescription = user.UserInfoDescription

            });
        }



        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserInfo(EditUserInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    ModelState.AddModelError("", "کاربر یافت نشد");
                }
                else
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.MelliCode = model.MelliCode;
                    var persianDateTime = PersianDateTime.Parse(model.BirthDate);
                    var date = persianDateTime.ToDateTime().ToUniversalTime();
                    user.BirthDate = date;
                    user.IsMelliCodeConfirmed = model.IsMelliCodeConfirmed;
                    user.IsBirthDateConfirmed = model.IsBirthDateConfirmed;
                    user.UserInfoDescription = model.UserInfoDescription;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            return PartialView("_EditUserInfo", model: model);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> AcceptUserInfo(int Id)
        {
            if (Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    user.IsMelliCodeConfirmed = isConfirmed.confirmed;
                    user.IsBirthDateConfirmed = isConfirmed.confirmed;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> RejectUserInfo(int Id=0)
        {
            if (Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    user.IsMelliCodeConfirmed = isConfirmed.notConfirmed;
                    user.IsBirthDateConfirmed = isConfirmed.notConfirmed;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }




        // auth Mobile
        [AjaxOnly]
        public async Task<IActionResult> RenderEditMobile([FromBody] ModelIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                ModelState.AddModelError("", "کاربر یافت نشد");
                return PartialView("_EditMobile", model: new EditMobileViewModel());
            }
            var _BirthDate = user.BirthDate.HasValue ? user.BirthDate.ToPersianYearMonthDay().ToString() : "";

            return PartialView("_EditMobile", model: new EditMobileViewModel
            {
                UserId = user.Id,
                MobileNumber = user.MobileNumber,
                MobileDescription = user.MobileDescription,
                IsMobileNumberConfirmed = user.IsMobileNumberConfirmed

            });
        }



        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMobile(EditMobileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    ModelState.AddModelError("", "کاربر یافت نشد");
                }
                else
                {
                    user.MobileNumber = model.MobileNumber;
                    user.MobileDescription = model.MobileDescription;
                    user.IsMobileNumberConfirmed = model.IsMobileNumberConfirmed;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            return PartialView("_EditMobile", model: model);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> AcceptMobile(int Id=0)
        {
            if (Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    user.IsMobileNumberConfirmed = isConfirmed.confirmed;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> RejectMobile(int Id=0)
        {
            if (Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    user.IsMobileNumberConfirmed = isConfirmed.notConfirmed;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }














        // auth card
        [AjaxOnly]
        public IActionResult RenderEditCard([FromBody] ModelGuidIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var card = _userBandCardServices.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (card == null)
            {
                ModelState.AddModelError("", "شماره کارت یافت نشد");
            }
            else
            {
                return PartialView("_EditCard", model: new EditCardViewModel
                {
                    UserId = card.UserId,
                    BankName = card.BankName,
                    CardNumber = card.CardNumber,
                    CardOwner = card.CardOwner,
                    CardStatus = card.CardStatus,
                    Description = card.Description,
                    Id = card.Id

                });

            }



            return PartialView("_EditCard", model: model);


        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCard(EditCardViewModel model)
        {
            if (ModelState.IsValid)
            {
                var card = _userBandCardServices.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
                if (card == null)
                {
                    ModelState.AddModelError("", "شماره کارت یافت نشد");
                }
                else
                {
                    card.BankName = model.BankName;
                    card.CardNumber = model.CardNumber;
                    card.CardOwner = model.CardOwner;
                    card.CardStatus = model.CardStatus;
                    card.Description = model.Description;


                    var result = await _userBandCardServices.UpdateAsync(card);
                    if (result)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            return PartialView("_EditCard", model: model);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> AcceptCard(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var card = _userBandCardServices.Query(c => c.Id == Id).Select(c => c).FirstOrDefault();
                if (card == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کارت یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    card.CardStatus = Entities.CardStatus.accepted;
                    var result = await _userBandCardServices.UpdateAsync(card);
                    if (result)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> RejectCard(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var card = _userBandCardServices.Query(c => c.Id == Id).Select(c => c).FirstOrDefault();
                if (card == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کارت یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    card.CardStatus = Entities.CardStatus.rejected;
                    var result = await _userBandCardServices.UpdateAsync(card);
                    if (result)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }

        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> DeleteCard(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var card = _userBandCardServices.Query(c => c.Id == Id).Select(c => c).FirstOrDefault();
                if (card == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کارت یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    card.CardStatus = Entities.CardStatus.Deleted;
                    var result = await _userBandCardServices.UpdateAsync(card);
                    if (result)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }





        // auth sheba
        [AjaxOnly]
        public IActionResult RenderEditSheba([FromBody] ModelGuidIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sheba = _userBandShebaServices.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (sheba == null)
            {
                ModelState.AddModelError("", "شماره شبا یافت نشد");
            }
            else
            {
                return PartialView("_EditSheba", model: new EditShebaViewModel
                {
                    UserId = sheba.UserId,

                    ShebaNumber = sheba.ShebaNumber,
                    ShebaOwner = sheba.ShebaOwner,
                    ShebaStatus = sheba.ShebaStatus,
                    Description = sheba.Description,
                    Id = sheba.Id

                });

            }



            return PartialView("_EditSheba", model: model);


        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSheba(EditShebaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sheba = _userBandShebaServices.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
                if (sheba == null)
                {
                    ModelState.AddModelError("", "شماره شبا یافت نشد");
                }
                else
                {

                    sheba.ShebaNumber = model.ShebaNumber;
                    sheba.ShebaOwner = model.ShebaOwner;
                    sheba.ShebaStatus = model.ShebaStatus;
                    sheba.Description = model.Description;


                    var result = await _userBandShebaServices.UpdateAsync(sheba);
                    if (result)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            return PartialView("_EditSheba", model: model);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> AcceptSheba(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var sheba = _userBandShebaServices.Query(c => c.Id == Id).Select(c => c).FirstOrDefault();
                if (sheba == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "شبا یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    sheba.ShebaStatus = Entities.ShebaStatus.accepted;
                    var result = await _userBandShebaServices.UpdateAsync(sheba);
                    if (result)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> RejectSheba(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var sheba = _userBandShebaServices.Query(c => c.Id == Id).Select(c => c).FirstOrDefault();
                if (sheba == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "شبا یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    sheba.ShebaStatus = Entities.ShebaStatus.rejected;
                    var result = await _userBandShebaServices.UpdateAsync(sheba);
                    if (result)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }

        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> DeleteSheba(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var sheba = _userBandShebaServices.Query(c => c.Id == Id).Select(c => c).FirstOrDefault();
                if (sheba == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "شبا یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    sheba.ShebaStatus = Entities.ShebaStatus.Deleted;
                    var result = await _userBandShebaServices.UpdateAsync(sheba);
                    if (result)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }








        // auth Doc
        [AjaxOnly]
        public async Task<IActionResult> RenderEditDoc([FromBody] ModelIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                ModelState.AddModelError("", "کاربر یافت نشد");
                return PartialView("_EditDoc", model: new EditDocViewModel());
            }

            return PartialView("_EditDoc", model: new EditDocViewModel
            {
                UserId = user.Id,
                IsDocPhotoConfirmed = user.IsDocPhotoConfirmed,
                DocDescription = user.DocDescription
            });
        }



        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoc(EditDocViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    ModelState.AddModelError("", "کاربر یافت نشد");
                }
                else
                {
                    user.IsDocPhotoConfirmed = model.IsDocPhotoConfirmed;
                    user.DocDescription = model.DocDescription;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            return PartialView("_EditDoc", model: model);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> AcceptDoc(int Id)
        {
            if (Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    var result = await _userManager.ConfirmDocFile(user.Id, isConfirmed.confirmed, "");
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> RejectDoc(int Id)
        {
            if (Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    var result = await _userManager.ConfirmDocFile(user.Id, isConfirmed.notConfirmed, "");
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }




        // auth Selfi
        [AjaxOnly]
        public async Task<IActionResult> RenderEditSelfi([FromBody] ModelIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                ModelState.AddModelError("", "کاربر یافت نشد");
                return PartialView("_EditSelfi", model: new EditSelfiViewModel());
            }

            return PartialView("_EditSelfi", model: new EditSelfiViewModel
            {
                UserId = user.Id,
                IsSelfiPhotoConfirmed = user.IsSelfiPhotoConfirmed,
                SelfiDescription = user.SelfiDescription
            });
        }



        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSelfi(EditSelfiViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    ModelState.AddModelError("", "کاربر یافت نشد");
                }
                else
                {
                    user.IsSelfiPhotoConfirmed = model.IsSelfiPhotoConfirmed;
                    user.SelfiDescription = model.SelfiDescription;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            return PartialView("_EditSelfi", model: model);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> AcceptSelfi(int Id)
        {
            if (Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    var result = await _userManager.ConfirmSelfiFile(user.Id, isConfirmed.confirmed, "");
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> RejectSelfi(int Id)
        {
            if (Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    var result = await _userManager.ConfirmSelfiFile(user.Id, isConfirmed.notConfirmed, "");
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }


        // auth Address
        [AjaxOnly]
        public async Task<IActionResult> RenderEditAddress([FromBody] ModelIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                ModelState.AddModelError("", "کاربر یافت نشد");
                return PartialView("_EditAddress", model: new EditAddressViewModel());
            }

            return PartialView("_EditAddress", model: new EditAddressViewModel
            {
                UserId = user.Id,
                AddressDescription = user.AddressDescription,
                IsLocationConfirmed = user.IsLocationConfirmed,
                IsPostalCodeConfirmed = user.IsPostalCodeConfirmed,
                Location = user.Location,
                PostalCode = user.PostalCode,


            });
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(EditAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    ModelState.AddModelError("", "کاربر یافت نشد");
                }
                else
                {
                    user.AddressDescription = model.AddressDescription;
                    user.IsLocationConfirmed = model.IsLocationConfirmed;
                    user.IsPostalCodeConfirmed = model.IsPostalCodeConfirmed;
                    user.Location = model.Location;
                    user.PostalCode = model.PostalCode;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            return PartialView("_EditAddress", model: model);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> AcceptAddress(int Id)
        {
            if (Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    var result = await _userManager.ConfirmAddress(user.Id, isConfirmed.confirmed, "");
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> RejectAddress(int Id)
        {
            if ( Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    var result = await _userManager.ConfirmAddress(user.Id, isConfirmed.notConfirmed, "");
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }






        //auth phone


        [AjaxOnly]
        public async Task<IActionResult> RenderEditPhone([FromBody] ModelIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                ModelState.AddModelError("", "کاربر یافت نشد");
                return PartialView("_EditPhone", model: new EditPhoneViewModel());
            }

            return PartialView("_EditPhone", model: new EditPhoneViewModel
            {
                UserId = user.Id,
                HomePhoneDescription = user.HomePhoneDescription,
                PhoneNumber = user.HomePhoneNumber,
                IsPhoneNumberConfirmed = user.IsPhoneNumberConfirmed


            });
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPhone(EditPhoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    ModelState.AddModelError("", "کاربر یافت نشد");
                }
                else
                {
                    user.HomePhoneDescription = model.HomePhoneDescription;
                    user.PhoneNumber = model.PhoneNumber;
                    user.IsPhoneNumberConfirmed = model.IsPhoneNumberConfirmed;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            return PartialView("_EditPhone", model: model);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> AcceptPhone(int Id)
        {
            if (Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    var result = await _userManager.ConfirmPhoneNumber(user.Id, isConfirmed.confirmed, "");
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }


        [Authorize(Roles = ConstantRoles.Admin)]
        [AjaxOnly]
        [HttpPost]
        public async Task<IActionResult> RejectPhone(int Id)
        {
            if (Id != 0)
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    var Warningmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.warning, MessageTitle = "پیام سیستم", MessageText = "کاربر یافت نشد" };
                    return PartialView("_Message", model: Warningmodel);
                }
                else
                {
                    var result = await _userManager.ConfirmPhoneNumber(user.Id, isConfirmed.notConfirmed, "");
                    if (result.Succeeded)
                    {
                        var Successmodel = new AlertMessageViewModel() { MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "وضعیت با موفقیت تغییر کرد" };
                        return PartialView("_Message", model: Successmodel);
                    }
                }
            }
            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطایی رخ داد" };
            return PartialView("_Message", model: Errormodel);
        }

    }
}