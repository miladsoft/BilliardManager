using System.Threading.Tasks;
using Billiard.Services.Contracts.Identity;
using Billiard.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;
using Billiard.Services;
using Billiard.Entities;
using Billiard.ViewModels;
using System.Linq;

namespace Billiard.Areas.Dashboard.ViewComponents
{
    public class UserInfoBoxViewComponent : ViewComponent
    {
 
        private readonly IApplicationUserManager _userManager;

        public UserInfoBoxViewComponent(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {          
            var user = await _userManager.FindByIdIncludeUserRolesAsync(userId);

            return View(viewName: "~/Areas/Dashboard/Views/Shared/Components/UserInfoBox/Default.cshtml", model: user);
                       
        }
    }
}