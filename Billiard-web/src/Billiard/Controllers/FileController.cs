using System.Text.RegularExpressions;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DNTCommon.Web.Core;
 namespace Billiard.Controllers
{
    [BreadCrumb(Title = "File", UseDefaultRouteUrl = true, Order = 0)]
    public class FileController : Controller
    {


        [BreadCrumb(Title = "File", Order = 1)]
        public IActionResult Image(string Id)
        {

            return null;

        }

      
    }
}