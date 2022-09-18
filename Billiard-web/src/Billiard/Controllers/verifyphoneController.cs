using System.Text.RegularExpressions;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DNTCommon.Web.Core;
using Billiard.Services.Contracts.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace Billiard.Controllers
{
    public class verifyphoneController : Controller
    {
        private readonly IApplicationUserManager _userManager;

        public verifyphoneController(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> uioyurtuyrtuioerutopertreerore(string phonenumber)
        {
            if (phonenumber.StartsWith("+98"))
            {
                phonenumber = phonenumber.Replace("+98", "");
            }
            if (phonenumber.StartsWith("0"))
            {
                phonenumber = phonenumber.Substring(1, 10);
            }
            if (phonenumber.StartsWith("98"))
            {
                phonenumber = phonenumber.Substring(2, 10);
            }
            var AllUser = await _userManager.GetAllUsersAsync();
           
            var q = AllUser.Where(c => c.HomePhoneNumber.Contains(phonenumber)).Select(c => c).FirstOrDefault();
            
            if (q != null)
                return Content(q.HomePhoneNumberCode);
            else
            {
                return Content("-");
            }
        }


    }
}