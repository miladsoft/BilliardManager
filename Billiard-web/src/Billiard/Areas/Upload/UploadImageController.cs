using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Billiard.Services.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Billiard.Areas.Upload;
using Microsoft.Net.Http.Headers;
using DNTCommon.Web.Core;


namespace Billiard.Areas.Upload.Controllers
{
    [Area(AreaConstants.UploadArea)]
    [Authorize]
    [DisplayName("Upload Image")]
    public class UploadImageController : Controller
    {
    
        private readonly IHostingEnvironment _hostingEnvironment;

   
        public UploadImageController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpPost]
        [DisplayName("Upload Image")]
        [Obsolete]
        public async Task<IActionResult> Upload()
        {
            var userId =  User.Identity.GetUserId();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", userId);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var files = HttpContext.Request.Form.Files;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse
                        (file.ContentDisposition).FileName.ToString();
                    var filePath = Path.Combine(path, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            return Ok();
        }
    }
}