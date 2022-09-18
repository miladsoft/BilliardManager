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
    public class UserInfoCommentViewComponent : ViewComponent
    {

        private readonly IApplicationUserManager _userManager;

        public UserInfoCommentViewComponent(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId, string date)
        {
            var user = await _userManager.FindByIdIncludeUserRolesAsync(userId);
            ViewData["Date"] = date;
            return View(viewName: "~/Areas/Dashboard/Views/Shared/Components/UserInfoComment/Default.cshtml", model: user);

        }
    }
}