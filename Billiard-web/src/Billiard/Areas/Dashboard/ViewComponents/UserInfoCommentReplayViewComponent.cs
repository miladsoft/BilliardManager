using System.Threading.Tasks;
using Billiard.Services.Contracts.Identity;
using Billiard.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;
using Billiard.Services;
using Billiard.Entities;
using Billiard.ViewModels;
using System.Linq;
using System;

namespace Billiard.Areas.Dashboard.ViewComponents
{
    public class UserInfoCommentReplayViewComponent : ViewComponent
    {

        private readonly IApplicationUserManager _userManager;

        public UserInfoCommentReplayViewComponent(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId, string date, string text)
        {
            var user = await _userManager.FindByIdIncludeUserRolesAsync(userId);
            ViewData["Date"] = date;
            ViewData["Text"] = text;
            return View(viewName: "~/Areas/Dashboard/Views/Shared/Components/UserInfoCommentReplay/Default.cshtml", model: user);

        }
    }
}