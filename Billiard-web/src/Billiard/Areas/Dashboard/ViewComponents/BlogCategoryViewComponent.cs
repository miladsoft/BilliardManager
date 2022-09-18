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
    public class BlogCategoryViewComponent : ViewComponent
    {
 
        private readonly IBlogPostCategoryServices _categoryManager;

        public BlogCategoryViewComponent(IBlogPostCategoryServices categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var catList =  _categoryManager.Query(c=>c.Name.ToLower()!="special" &&c.IsDelete!=true).Select(c=>new BlogPostCategoryViewModel(){
                 BlogPosts=c.BlogPosts,
                 Id=c.Id,
                 ImageFileName=c.ImageFileName,
                 Name=c.Name,
                 Title=c.Title
            }).ToList();
            return View(viewName: "~/Areas/Dashboard/Views/Shared/Components/BlogCategory/Default.cshtml",
                        model: catList);
        }
    }
}