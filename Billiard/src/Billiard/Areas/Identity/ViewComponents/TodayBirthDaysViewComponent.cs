using Billiard.Services.Contracts.Identity;
using Billiard.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Billiard.Areas.Identity.ViewComponents;

public class TodayBirthDaysViewComponent : ViewComponent
{
    private readonly ISiteStatService _siteStatService;

    public TodayBirthDaysViewComponent(ISiteStatService siteStatService)
    {
        _siteStatService = siteStatService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var usersList = await _siteStatService.GetTodayBirthdayListAsync();
        var usersAverageAge = await _siteStatService.GetUsersAverageAge();

        return View("~/Areas/Identity/Views/Shared/Components/TodayBirthDays/Default.cshtml",
            new TodayBirthDaysViewModel
            {
                Users = usersList,
                AgeStat = usersAverageAge
            });
    }
}