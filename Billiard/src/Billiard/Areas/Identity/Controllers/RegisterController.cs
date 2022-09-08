﻿using Billiard.Common.IdentityToolkit;
using Billiard.Entities.Identity;
using Billiard.Services.Contracts.Identity;
using Billiard.ViewModels.Identity;
using Billiard.ViewModels.Identity.Emails;
using Billiard.ViewModels.Identity.Settings;
using DNTBreadCrumb.Core;
using DNTCaptcha.Core;
using DNTCommon.Web.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Language = DNTCaptcha.Core.Language;

namespace Billiard.Areas.Identity.Controllers;

[Area(AreaConstants.IdentityArea), AllowAnonymous, BreadCrumb(Title = "ثبت نام", UseDefaultRouteUrl = true, Order = 0)]
public class RegisterController : Controller
{
    private readonly IEmailSender _emailSender;
    private readonly IPasswordValidator<User> _passwordValidator;
    private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
    private readonly IApplicationUserManager _userManager;
    private readonly IUserValidator<User> _userValidator;

    public RegisterController(
        IApplicationUserManager userManager,
        IPasswordValidator<User> passwordValidator,
        IUserValidator<User> userValidator,
        IEmailSender emailSender,
        IOptionsSnapshot<SiteSettings> siteOptions)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _passwordValidator = passwordValidator ?? throw new ArgumentNullException(nameof(passwordValidator));
        _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        _siteOptions = siteOptions ?? throw new ArgumentNullException(nameof(siteOptions));
    }

    /// <summary>
    ///     For [Remote] validation
    /// </summary>
    [AjaxOnly, HttpPost, ValidateAntiForgeryToken, ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> ValidateUsername(string username, string email)
    {
        var result = await _userValidator.ValidateAsync(
            (UserManager<User>)_userManager, new User { UserName = username, Email = email });
        return Json(result.Succeeded ? "true" : result.DumpErrors(true));
    }

    /// <summary>
    ///     For [Remote] validation
    /// </summary>
    [AjaxOnly, HttpPost, ValidateAntiForgeryToken, ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> ValidatePassword(string password, string username)
    {
        var result = await _passwordValidator.ValidateAsync(
            (UserManager<User>)_userManager, new User { UserName = username }, password);
        return Json(result.Succeeded ? "true" : result.DumpErrors(true));
    }

    [BreadCrumb(Title = "تائید ایمیل", Order = 1)]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return View("Error");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return View("NotFound");
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);
        return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
    }

    [BreadCrumb(Title = "تائیدیه ایمیل", Order = 1)]
    public IActionResult ConfirmedRegisteration()
    {
        return View();
    }

    [BreadCrumb(Title = "ایندکس", Order = 1)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken, ValidateDNTCaptcha(CaptchaGeneratorLanguage = Language.Persian,
         CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbers)]
    public async Task<IActionResult> Index(RegisterViewModel model)
    {
        if (model is null)
        {
            return View("Error");
        }

        if (ModelState.IsValid)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (_siteOptions.Value.EnableEmailConfirmation)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //ControllerExtensions.ShortControllerName<RegisterController>(), //TODO: use everywhere .................

                    await _emailSender.SendEmailAsync(
                        user.Email,
                        "لطفا اکانت خود را تائید کنید",
                        "~/Areas/Identity/Views/EmailTemplates/_RegisterEmailConfirmation.cshtml",
                        new RegisterEmailConfirmationViewModel
                        {
                            User = user,
                            EmailConfirmationToken = code,
                            EmailSignature = _siteOptions.Value.Smtp.FromName,
                            MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                        });

                    return RedirectToAction(nameof(ConfirmYourEmail));
                }

                return RedirectToAction(nameof(ConfirmedRegisteration));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [BreadCrumb(Title = "ایمیل خود را تائید کنید", Order = 1)]
    public IActionResult ConfirmYourEmail()
    {
        return View();
    }
}