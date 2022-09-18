using Billiard.Common.IdentityToolkit;
using Billiard.Entities.Identity;
using Billiard.Services.Contracts.Identity;
using Billiard.Services.Identity;
using Billiard.ViewModels.Identity;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Billiard.Areas.Dashboard.Controllers
{
    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.DashboardArea)]
    [BreadCrumb(Title = "مدیریت کاربران", UseDefaultRouteUrl = true, Order = 0)]
    public class UsersManagerController : Controller
    {
        private const int DefaultPageSize = 7;

        private readonly IApplicationRoleManager _roleManager;
        private readonly IApplicationUserManager _userManager;

        public UsersManagerController(
            IApplicationUserManager userManager,
            IApplicationRoleManager roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ActivateUserEmailStat(int userId)
        {
            User thisUser = null;
            var result = await _userManager.UpdateUserAndSecurityStampAsync(
                userId, user =>
                {
                    user.EmailConfirmed = true;
                    thisUser = user;
                });
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await returnUserPartialView(thisUser);
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ChangeUserLockoutMode(int userId, bool activate)
        {
            User thisUser = null;
            var result = await _userManager.UpdateUserAndSecurityStampAsync(
                userId, user =>
                {
                    user.LockoutEnabled = activate;
                    thisUser = user;
                });
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await returnUserPartialView(thisUser);
        }


        
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ChangeUserBlueTick(int userId, bool activate)
        {
            User thisUser = null;
            var result = await _userManager.UpdateUserAndSecurityStampAsync(
                userId, user =>
                {
                    user.IsBlueTick = activate;
                    thisUser = user;
                });
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await returnUserPartialView(thisUser);
        }


        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ChangeUserRoles(int userId, int[] roleIds)
        {
            
            User thisUser = null;
            var result = await _userManager.AddOrUpdateUserRolesAsync(
                userId, roleIds, user => thisUser = user);
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await returnUserPartialView(thisUser);
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ChangeUserStat(int userId, bool activate)
        {
            User thisUser = null;
            var result = await _userManager.UpdateUserAndSecurityStampAsync(
                userId, user =>
                        {
                            user.IsActive = activate;
                            thisUser = user;
                        });
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await returnUserPartialView(thisUser);
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ChangeUserTwoFactorAuthenticationStat(int userId, bool activate)
        {
            User thisUser = null;
            var result = await _userManager.UpdateUserAndSecurityStampAsync(
                userId, user =>
                {
                    user.TwoFactorEnabled = activate;
                    thisUser = user;
                });
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await returnUserPartialView(thisUser);
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> EndUserLockout(int userId)
        {
            User thisUser = null;
            var result = await _userManager.UpdateUserAndSecurityStampAsync(
                userId, user =>
                {
                    user.LockoutEnd = null;
                    thisUser = user;
                });
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await returnUserPartialView(thisUser);
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index(int? page = 1)
        {
            var model = await _userManager.GetPagedUsersListAsync(
                pageNumber: page.Value - 1,
                recordsPerPage: DefaultPageSize,
                sortByField: "Id",
                sortOrder: SortOrder.Descending,
                showAllUsers: true);

            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            if (HttpContext.Request.IsAjaxRequest())
            {
                return PartialView("_UsersList", model);
            }
            return View(model);
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> SearchUsers(SearchUsersViewModel model)
        {
            var pagedUsersList = await _userManager.GetPagedUsersListAsync(
                model: model,
                pageNumber: 0);

            pagedUsersList.Paging.CurrentPage = 1;
            pagedUsersList.Paging.ItemsPerPage = model.MaxNumberOfRows;
            pagedUsersList.Paging.ShowFirstLast = true;

            model.PagedUsersList = pagedUsersList;
            return PartialView("_SearchUsers", model);
        }

        private async Task<IActionResult> returnUserPartialView(User thisUser)
        {
            var roles = await _roleManager.GetAllCustomRolesAsync();
            return PartialView("~/Areas/Dashboard/Views/User/_UserItem.cshtml",
                new UserItemViewModel
                {
                    User = thisUser,
                    ShowAdminParts = true,
                    Roles = roles,
                    ActiveTab = UserItemActiveTab.UserAdmin
                });
        }
    }
}