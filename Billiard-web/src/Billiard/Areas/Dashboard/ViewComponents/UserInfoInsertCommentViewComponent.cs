using System.Threading.Tasks;
using Billiard.Services.Contracts.Identity;
using Billiard.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;
using Billiard.Services;
using Billiard.Entities;
using Billiard.ViewModels;
using System.Linq;
using Billiard.Entities.Identity;

namespace Billiard.Areas.Dashboard.ViewComponents
{
    public class UserInfoInsertCommentViewComponent : ViewComponent
    {

        private readonly IApplicationUserManager _userManager;

        public UserInfoInsertCommentViewComponent(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {
            User user;
            if (userId == 0)
            {

                user = _userManager.GetCurrentUser();
            }
            else
            {
                user = await _userManager.FindByIdIncludeUserRolesAsync(userId);

            }

            return View(viewName: "~/Areas/Dashboard/Views/Shared/Components/UserInfoInsertComment/Default.cshtml", model: user);

        }
    }
}