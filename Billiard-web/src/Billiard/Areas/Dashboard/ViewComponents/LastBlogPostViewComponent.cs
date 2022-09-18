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
    public class LastBlogPostViewComponent : ViewComponent
    {
 
        private readonly IBlogPostServices _postManager;

        public LastBlogPostViewComponent(IBlogPostServices postManager)
        {
            _postManager = postManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var catList =  _postManager.Query(c=>c.IsSpecial!=true &&c.IsDelete!=true).OrderBy(x=>x.OrderByDescending(c=>c.Id)).Select(c=>c).Take(5).ToList();
            return View(viewName: "~/Areas/Dashboard/Views/Shared/Components/LastBlogPost/Default.cshtml",
                        model: catList);
        }
    }
}