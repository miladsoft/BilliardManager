using System.Linq;
using Billiard.Common.GuardToolkit;
using Billiard.Common.IdentityToolkit;
using Billiard.Entities.Identity;
using Billiard.Services.Contracts.Identity;
using Billiard.Services.Identity;
using Billiard.ViewModels.Identity.Emails;
using Billiard.ViewModels.Identity.Settings;
using Billiard.ViewModels.Identity;
using DNTBreadCrumb.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;
using System;
using DNTCommon.Web.Core;
using MD.PersianDateTime.Core;
using Billiard.Services;
using Billiard.ViewModels;
using Billiard.Entities;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.Security.AccessControl;

namespace Billiard.Areas.Dashboard.Controllers
{
    [Authorize]
    [Area(AreaConstants.DashboardArea)]
    [BreadCrumb(Title = "مشخصات کاربری", UseDefaultRouteUrl = true, Order = 0)]
    public class UserProfileController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IProtectionProviderService _protectionProviderService;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
        private readonly IUsedPasswordsService _usedPasswordsService;
        private readonly IApplicationUserManager _userManager;
        private readonly IUsersPhotoService _usersPhotoService;
        private readonly IUserValidator<User> _userValidator;
        private readonly ILogger<UserProfileController> _logger;
        private readonly ISmsSender _smsSender;
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IUserBandCardServices _userBandCardServices;
        private readonly IUserBandShebaServices _userBandShebaServices;


        public UserProfileController(
            IApplicationUserManager userManager,
            IApplicationRoleManager roleManager,
            IApplicationSignInManager signInManager,
            IProtectionProviderService protectionProviderService,
            IUserValidator<User> userValidator,
            IUsedPasswordsService usedPasswordsService,
            IUsersPhotoService usersPhotoService,
            IOptionsSnapshot<SiteSettings> siteOptions,
            IEmailSender emailSender,
            ILogger<UserProfileController> logger,
            ISmsSender smsSender,
            IUserBandCardServices userBandCardServices,
            IUserBandShebaServices userBandShebaServices,
            IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _protectionProviderService = protectionProviderService ?? throw new ArgumentNullException(nameof(protectionProviderService));
            _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
            _usedPasswordsService = usedPasswordsService ?? throw new ArgumentNullException(nameof(usedPasswordsService));
            _usersPhotoService = usersPhotoService ?? throw new ArgumentNullException(nameof(usersPhotoService));
            _siteOptions = siteOptions ?? throw new ArgumentNullException(nameof(siteOptions));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _smsSender = smsSender ?? throw new ArgumentNullException(nameof(smsSender));
            _userBandCardServices = userBandCardServices ?? throw new ArgumentNullException(nameof(userBandCardServices));
            _userBandShebaServices = userBandShebaServices ?? throw new ArgumentNullException(nameof(userBandShebaServices));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));



        }

        [Authorize(Roles = ConstantRoles.Admin)]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> AdminEdit(int? id)
        {
            if (!id.HasValue)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            return await renderForm(user, isAdminEdit: true);
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetCurrentUserAsync();
            return await renderForm(user, isAdminEdit: false);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserProfileViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var pid = _protectionProviderService.Decrypt(model.Pid);
                if (string.IsNullOrWhiteSpace(pid))
                {
                    return View("Error");
                }

                if (pid != _userManager.GetCurrentUserId() &&
                    !_roleManager.IsCurrentUserInRole(ConstantRoles.Admin))
                {
                    _logger.LogWarning($"سعی در دسترسی غیرمجاز به ویرایش اطلاعات کاربر {pid}");
                    return View("Error");
                }

                var user = await _userManager.FindByIdAsync(pid);
                if (user == null)
                {
                    return View("NotFound");
                }

                user.FirstName = model.FirstName.ApplyCorrectYeKe();
                user.LastName = model.LastName.ApplyCorrectYeKe();
                user.IsEmailPublic = model.IsEmailPublic;
                user.TwoFactorEnabled = model.TwoFactorEnabled;
                user.Location = model.Location.ApplyCorrectYeKe();
                user.IsMelliCodeConfirmed = isConfirmed.waitingforconfirmation;
                user.IsBirthDateConfirmed = isConfirmed.waitingforconfirmation;

                updateUserBirthDate(model, user);
                updateUserMelliCode(model, user);

                if (!await updateUserName(model, user))
                {
                    return View(viewName: nameof(Index), model: model);
                }

                if (!await updateUserAvatarImage(model, user))
                {
                    return View(viewName: nameof(Index), model: model);
                }

                if (!await updateUserEmail(model, user))
                {
                    return View(viewName: nameof(Index), model: model);
                }

                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    if (!model.IsAdminEdit)
                    {
                        // reflect the changes in the current user's Identity cookie
                        await _signInManager.RefreshSignInAsync(user);
                    }

                    try
                    {

                        await _emailSender.SendEmailAsync(
                                email: user.Email,
                                subject: "اطلاع رسانی به روز رسانی مشخصات کاربری",
                                viewNameOrPath: "~/Areas/Dashboard/Views/EmailTemplates/_UserProfileUpdateNotification.cshtml",
                                model: new UserProfileUpdateNotificationViewModel
                                {
                                    User = user,
                                    EmailSignature = _siteOptions.Value.Smtp.FromName,
                                    MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                                });
                    }
                    catch
                    {
                    }
                    if (model.IsAdminEdit)
                    {
                        return RedirectToAction(nameof(AdminEdit), "UserProfile", routeValues: new { id = user.Id });
                    }
                    if (!model.IsAdminEdit)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                ModelState.AddModelError("", updateResult.DumpErrors(useHtmlNewLine: true));
            }
            return View(viewName: nameof(Index), model: model);
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateUsername(string username, string email, string pid)
        {
            pid = _protectionProviderService.Decrypt(pid);
            if (string.IsNullOrWhiteSpace(pid))
            {
                return Json("اطلاعات وارد شده معتبر نیست.");
            }

            var user = await _userManager.FindByIdAsync(pid);
            user.UserName = email;
            user.Email = email;

            var result = await _userValidator.ValidateAsync((UserManager<User>)_userManager, user);
            return Json(result.Succeeded ? "true" : result.DumpErrors(useHtmlNewLine: true));
        }

        private static void updateUserMelliCode(UserProfileViewModel model, User user)
        {
            if (!string.IsNullOrEmpty(model.MelliCode.ToString()))
            {
                var IsValid = model.MelliCode.IsValidIranianNationalCode();
                if (IsValid)
                {
                    user.MelliCode = model.MelliCode;
                }
            }
            else
            {
                user.MelliCode = null;
            }
        }
        private static void updateUserBirthDate(UserProfileViewModel model, User user)
        {
            if (!string.IsNullOrEmpty(model.BirthDate.ToString()))
            {
                try
                {
                    var persianDateTime = PersianDateTime.Parse(model.BirthDate);
                    var date = persianDateTime.ToDateTime();
                    user.BirthDate = date;
                }
                catch
                {

                }

            }
            else
            {
                user.BirthDate = null;
            }
        }

        private async Task<IActionResult> renderForm(User user, bool isAdminEdit)
        {
            _usersPhotoService.SetUserDefaultPhoto(user);
            var usercards = _userBandCardServices.Query(c => c.UserId == user.Id).Select(c => c).ToList();

            var UserShebas = _userBandShebaServices.Query(c => c.UserId == user.Id).Select(c => c).ToList();

            var userProfile = new UserProfileViewModel
            {
                Id = user.Id,
                IsAdminEdit = isAdminEdit,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                IsMobileNumberConfirmed = user.IsMobileNumberConfirmed,
                MelliCode = user.MelliCode,
                BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value.LocalDateTime.ToPersianYearMonthDay().ToString() : "",
                PhotoFileName = user.PhotoFileName,
                Location = user.Location,
                IsLocationConfirmed = user.IsLocationConfirmed,
                PostalCode = user.PostalCode,
                AddressDescription = user.AddressDescription,
                CardDescription = user.CardDescription,
                IsPostalCodeConfirmed = user.IsPostalCodeConfirmed,
                ShebaDescription = user.ShebaDescription,
                HomePhoneDescription = user.HomePhoneDescription,
                PhoneNumber = user.HomePhoneNumber,
                IsPhoneNumberConfirmed = user.IsPhoneNumberConfirmed,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Pid = _protectionProviderService.Encrypt(user.Id.ToString()),
                IsEmailPublic = user.IsEmailPublic,
                TwoFactorEnabled = user.TwoFactorEnabled,
                CardNumbers = usercards,
                ShebaNumbers = UserShebas,
                IsPasswordTooOld = await _usedPasswordsService.IsLastUserPasswordTooOldAsync(user.Id),
                IsBirthDateConfirmed = user.IsBirthDateConfirmed,
                IsMelliCodeConfirmed = user.IsMelliCodeConfirmed,
                UserInfoDescription = user.UserInfoDescription,
                MobileDescription = user.MobileDescription,
                DocPhotoFileName = user.DocPhotoFileName,
                DocDescription = user.DocDescription,
                SelfiPhotoFileName = user.SelfiPhotoFileName,
                SelfiDescription = user.SelfiDescription,
                IsSelfiPhotoConfirmed = user.IsSelfiPhotoConfirmed,
                IsDocPhotoConfirmed = user.IsDocPhotoConfirmed,


            };



            return View(viewName: nameof(Index), model: userProfile);
        }

        private async Task<bool> updateUserAvatarImage(UserProfileViewModel model, User user)
        {
            _usersPhotoService.SetUserDefaultPhoto(user);

            var photoFile = model.Photo;
            if (photoFile?.Length > 0)
            {
                var imageOptions = _siteOptions.Value.UserAvatarImageOptions;
                if (!photoFile.IsValidImageFile(maxWidth: imageOptions.MaxWidth, maxHeight: imageOptions.MaxHeight))
                {
                    this.ModelState.AddModelError("",
                        $"حداکثر اندازه تصویر قابل ارسال {imageOptions.MaxHeight} در {imageOptions.MaxWidth} پیکسل است");
                    model.PhotoFileName = user.PhotoFileName;
                    return false;
                }

                var uploadsRootFolder = _usersPhotoService.GetUsersAvatarsFolderPath();
                var photoFileName = $"{user.Id}{Path.GetExtension(photoFile.FileName)}";
                var filePath = Path.Combine(uploadsRootFolder, photoFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photoFile.CopyToAsync(fileStream);
                }
                user.PhotoFileName = photoFileName;
            }
            return true;
        }

        private async Task<bool> updateUserEmail(UserProfileViewModel model, User user)
        {
            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                var userValidator =
                    await _userValidator.ValidateAsync((UserManager<User>)_userManager, user);
                if (!userValidator.Succeeded)
                {
                    ModelState.AddModelError("", userValidator.DumpErrors(useHtmlNewLine: true));
                    return false;
                }

                user.EmailConfirmed = false;

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _emailSender.SendEmailAsync(
                    email: user.Email,
                    subject: "لطفا اکانت خود را تائید کنید",
                    viewNameOrPath: "~/Areas/Dashboard/Views/EmailTemplates/_RegisterEmailConfirmation.cshtml",
                    model: new RegisterEmailConfirmationViewModel
                    {
                        User = user,
                        EmailConfirmationToken = code,
                        EmailSignature = _siteOptions.Value.Smtp.FromName,
                        MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                    });
            }

            return true;
        }

        private async Task<bool> updateUserName(UserProfileViewModel model, User user)
        {
            if (user.UserName != model.Email)
            {
                user.UserName = model.Email;
                var userValidator =
                    await _userValidator.ValidateAsync((UserManager<User>)_userManager, user);
                if (!userValidator.Succeeded)
                {
                    ModelState.AddModelError("", userValidator.DumpErrors(useHtmlNewLine: true));
                    return false;
                }
            }
            return true;
        }


        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMobile([FromBody] string MobileNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(error: "خطایی رخ داده است");
            }
            if (!MobileNumber.IsValidIranianMobileNumber())
            {
                return BadRequest(error: "لطفا شماره موبایل خود را به درستی وارد نمایید");
            }
            var currentUser = _userManager.GetCurrentUser();
            var result = await _userManager.AddMobile(currentUser.Id, MobileNumber);
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(currentUser, MobileNumber);
            await _smsSender.SendSmsAsync(41193, MobileNumber, code);
            return Json(new { success = true });
        }



        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmMobile([FromBody] string MobileConfirmedCode)
        {
            var currentUser = _userManager.GetCurrentUser();
            var userId = currentUser.Id;
            var code = await _userManager.VerifyChangePhoneNumberTokenAsync(currentUser, MobileConfirmedCode, currentUser.MobileNumber);
            if (code)
            {
                var result = await _userManager.ConfirmMobile(userId, isConfirmed.confirmed);
                if (!result.Succeeded)
                {
                    return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
                }
            }
            else
            {
                return BadRequest(error: "کد تایید شما اشتباه است");
            }
            return Json(new { success = true });
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCardNumber([FromBody] string CardNumber)
        {
            if (!string.IsNullOrEmpty(CardNumber))
            {
                var Card = CardNumber.Replace(" ", "").Replace("-", "");
                if (!Card.IsValidIranShetabNumber())
                {
                    return BadRequest(error: "شماره کارت شما اشتباه است");
                }

                var user = _userManager.GetCurrentUser();
                var q = _userBandCardServices.Query(c => c.CardNumber == Card).Select(c => c);
                if (q.Any())
                {
                    var q_card = q.FirstOrDefault();
                    if (user.Id == q_card.UserId)
                    {
                        if (q_card.CardStatus == CardStatus.Deleted)
                        {
                            q_card.CardStatus = CardStatus.accepted;
                            var updateresult = await _userBandCardServices.UpdateAsync(q_card);
                            if (updateresult)
                            {
                                return Json(new { success = true });
                            }
                        }
                        else
                        {
                            return BadRequest(error: "این شماره کارت قبلا ثبت شده است");
                        }

                    }
                    else
                    {
                        return BadRequest(error: "شماره کارت وارد شده متعلق به شما نمی باشد");
                    }
                }


                var CheckCard = await _userBandCardServices.CheckCardNumber(Card, user.DisplayName);
                if (CheckCard)
                {
                    var bankname = _userBandCardServices.GetBankName(Card);

                    await _userBandCardServices.InsertAsync(new Entities.UserBankCards { CardNumber = Card, CardStatus = Entities.CardStatus.accepted, UserId = user.Id, CardOwner = user.DisplayName, BankName = bankname });
                    return Json(new { success = true });
                }
            }
            else
            {
                return BadRequest(error: "شماره کارت را وارد کنید");
            }
            return BadRequest(error: "شماره کارت وارد شده متعلق به شما نمی باشد");
        }


        [AjaxOnly]
        public IActionResult RenderDeleteCard([FromBody] ModelGuidIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model == null)
            {
                return BadRequest("model is null.");
            }

            var card = _userBandCardServices.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (card == null)
            {
                ModelState.AddModelError("", "شماره کارت یافت نشد");
                return PartialView("_DeleteCard", model: new UserBankCards());
            }
            return PartialView("_DeleteCard", model: new UserBankCards { Id = card.Id, CardNumber = card.CardNumber });
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCard(UserBankCards model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id != Guid.Empty)
            {
                var card = _userBandCardServices.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
                if (card == null)
                {
                    ModelState.AddModelError("", "شماره کارت یافت نشد");
                }
                else
                {
                    card.CardStatus = CardStatus.Deleted;
                    var result = await _userBandCardServices.UpdateAsync(card);
                    if (result)
                    {
                        return Json(new { success = true });
                    }
                }
                return PartialView("_DeleteCard", model: model);
            }
            return BadRequest("model is null.");
        }


        //Sheba





        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShebaNumber([FromBody] string ShebaNumber)
        {
            if (!string.IsNullOrEmpty(ShebaNumber))
            {
                var sheba = ShebaNumber.Replace(" ", "").Replace("-", "").ToUpper().Replace("ir", "");
                if (!sheba.IsValidIranShebaNumber())
                {
                    return BadRequest(error: "شماره شبا شما اشتباه است");
                }

                var user = _userManager.GetCurrentUser();
                var q = _userBandShebaServices.Query(c => c.ShebaNumber == sheba).Select(c => c);
                if (q.Any())
                {
                    var q_sheba = q.FirstOrDefault();
                    if (user.Id == q_sheba.UserId)
                    {
                        if (q_sheba.ShebaStatus == ShebaStatus.Deleted)
                        {
                            q_sheba.ShebaStatus = ShebaStatus.accepted;
                            var updateresult = await _userBandShebaServices.UpdateAsync(q_sheba);
                            if (updateresult)
                            {
                                return Json(new { success = true });
                            }
                        }
                        else
                        {
                            return BadRequest(error: "این شماره شبا قبلا ثبت شده است");
                        }

                    }
                    else
                    {
                        return BadRequest(error: "شماره شبا وارد شده متعلق به شما نمی باشد");
                    }
                }


                var CheckSheba = await _userBandShebaServices.CheckShebaNumber(sheba.Replace("IR", ""), user.DisplayName);
                if (CheckSheba)
                {

                    await _userBandShebaServices.InsertAsync(new Entities.UserBankShebas { ShebaNumber = sheba, ShebaStatus = Entities.ShebaStatus.accepted, UserId = user.Id, ShebaOwner = user.DisplayName });
                    return Json(new { success = true });
                }
            }
            else
            {
                return BadRequest(error: "شماره شبا را وارد کنید");
            }
            return BadRequest(error: "شماره شبا وارد شده متعلق به شما نمی باشد");
        }


        [AjaxOnly]
        public IActionResult RenderDeleteSheba([FromBody] ModelGuidIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model == null)
            {
                return BadRequest("model is null.");
            }

            var sheba = _userBandShebaServices.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (sheba == null)
            {
                ModelState.AddModelError("", "شماره شبا یافت نشد");
                return PartialView("_DeleteSheba", model: new UserBankShebas());
            }
            return PartialView("_DeleteSheba", model: new UserBankShebas { Id = sheba.Id, ShebaNumber = sheba.ShebaNumber });
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSheba(UserBankCards model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id == Guid.Empty)
            {
                return BadRequest("model is null.");
            }

            var sheba = _userBandShebaServices.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (sheba == null)
            {
                ModelState.AddModelError("", "شماره شبا یافت نشد");
            }
            else
            {
                sheba.ShebaStatus = ShebaStatus.Deleted;
                var result = await _userBandShebaServices.UpdateAsync(sheba);
                if (result)
                {
                    return Json(new { success = true });
                }
            }
            return PartialView("_DeleteSheba", model: model);
        }



        [AjaxOnly]
        [HttpPost]
        // [ValidateAntiForgeryToken]      
        public async Task<IActionResult> UploadDoc()
        {
            var userId = User.Identity.GetUserId();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads","AuthPPCUsers", userId);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var files = HttpContext.Request.Form.Files;

            var file = files[0];
            if (file != null)
            {
                if (file.ContentType.ToLower() != "image/jpg" &&
                    file.ContentType.ToLower() != "image/jpeg" &&
                    file.ContentType.ToLower() != "image/pjpeg" &&
                    file.ContentType.ToLower() != "image/gif" &&
                    file.ContentType.ToLower() != "image/x-png" &&
                    file.ContentType.ToLower() != "image/png")
                {
                    this.ModelState.AddModelError("", $"لطفا فقط عکس انتخاب کنید");

                    var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا فقط عکس انتخاب کنید" };
                    return PartialView("_Message", model: Errormodel);
                }
                else
                {
                    if (file.Length > 0)
                    {
                        long fileSizeInBytes = file.Length;

                        long fileSizeInKB = fileSizeInBytes / 1024;

                        long fileSizeInMB = fileSizeInKB / 1024;
                        if (fileSizeInMB < 10)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse
                                (file.ContentDisposition).FileName.ToString();

                            var fname = Guid.NewGuid().ToString().Replace("-", "") + "-" + fileName.Replace("\"","");

                            var filePath = Path.Combine(path, fname);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            var IntUID = int.Parse(userId);
                            var res = _userManager.AddDocFile(IntUID, fname);
                            if (res.Result.Succeeded)
                            {
                                return Ok();

                            }


                        }
                        else
                        {
                            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا عکس کمتر از 10 مگابایت انتخاب کنید" };
                            return PartialView("_Message", model: Errormodel);
                        }
                    }
                }
            }
            var Errormodel2 = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطا در بارگذاری عکس رخ داد" };
            return PartialView("_Message", model: Errormodel2);
        }



        [AjaxOnly]
        [HttpPost]
        // [ValidateAntiForgeryToken]      
        public async Task<IActionResult> UploadSelfi()
        {
            var userId = User.Identity.GetUserId();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads","AuthPPCUsers", userId);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var files = HttpContext.Request.Form.Files;

            var file = files[0];
            if (file != null)
            {
                if (file.ContentType.ToLower() != "image/jpg" &&
                    file.ContentType.ToLower() != "image/jpeg" &&
                    file.ContentType.ToLower() != "image/pjpeg" &&
                    file.ContentType.ToLower() != "image/gif" &&
                    file.ContentType.ToLower() != "image/x-png" &&
                    file.ContentType.ToLower() != "image/png")
                {
                    this.ModelState.AddModelError("", $"لطفا فقط عکس انتخاب کنید");

                    var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا فقط عکس انتخاب کنید" };
                    return PartialView("_Message", model: Errormodel);
                }
                else
                {
                    if (file.Length > 0)
                    {
                        long fileSizeInBytes = file.Length;

                        long fileSizeInKB = fileSizeInBytes / 1024;

                        long fileSizeInMB = fileSizeInKB / 1024;
                        if (fileSizeInMB < 10)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse
                                (file.ContentDisposition).FileName.ToString();

                            var fname = Guid.NewGuid().ToString().Replace("-", "") + "-" + fileName.Replace("\"","");

                            var filePath = Path.Combine(path, fname);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            var IntUID = int.Parse(userId);
                            var res = _userManager.AddSelfiFile(IntUID, fname);
                            if (res.Result.Succeeded)
                            {
                                return Ok();

                            }


                        }
                        else
                        {
                            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا عکس کمتر از 10 مگابایت انتخاب کنید" };
                            return PartialView("_Message", model: Errormodel);
                        }
                    }
                }
            }
            var Errormodel2 = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "خطا در بارگذاری عکس رخ داد" };
            return PartialView("_Message", model: Errormodel2);
        }




        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress([FromBody] string PostalCode, [FromBody] string Location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(error: "خطایی رخ داده است");
            }

            var currentUser = _userManager.GetCurrentUser();
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(currentUser, PostalCode);


            var result = await _userManager.AddAddress(currentUser.Id, Location, PostalCode, code);
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return Json(new { success = true });
        }




        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAddress([FromBody] string AddressConfirmedCode)
        {
            var currentUser = _userManager.GetCurrentUser();
            var userId = currentUser.Id;

            if (currentUser.LocationCode == AddressConfirmedCode)
            {
                var result = await _userManager.ConfirmAddress(userId, isConfirmed.confirmed, "");
                if (!result.Succeeded)
                {
                    return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
                }
            }
            else
            {
                var result = await _userManager.ConfirmAddress(userId, isConfirmed.notConfirmed, "");
                if (result.Succeeded)
                {
                    return BadRequest(error: "کد تایید شما اشتباه است");
                }
            }
            return Json(new { success = true });
        }







        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePhone([FromBody] string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                return BadRequest(error: "خطایی رخ داده است");
            }

            var currentUser = _userManager.GetCurrentUser();
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(currentUser, PhoneNumber);
            var result = await _userManager.AddPhoneNumber(currentUser.Id, PhoneNumber, code.Substring(0, 4));
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return Json(new { success = true });
        }



        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPhone([FromBody] string PhoneConfirmedCode)
        {
            var currentUser = _userManager.GetCurrentUser();
            var userId = currentUser.Id;
            if (currentUser.HomePhoneNumberCode == PhoneConfirmedCode)
            {
                var result = await _userManager.ConfirmPhoneNumber(userId, isConfirmed.confirmed, "");
                if (!result.Succeeded)
                {
                    return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
                }
            }
            else
            {
                return BadRequest(error: "کد تایید شما اشتباه است");
            }
            return Json(new { success = true });
        }




        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTwoFactorEnabled([FromBody] bool TwoFactorEnabled)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(error: "خطایی رخ داده است");
            }

            var currentUser = _userManager.GetCurrentUser();

            currentUser.TwoFactorEnabled = TwoFactorEnabled;
            var result = await _userManager.UpdateAsync(currentUser);
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return Json(new { success = true });
        }


    }
}
