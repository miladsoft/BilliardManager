using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Billiard.Services.Contracts.Identity;
using Billiard.Services.Identity;
using Billiard.ViewModels;
using Billiard.ViewModels.Identity;
using Billiard.Services;
using Billiard.Entities;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using ClosedXML.Excel;
using DNTPersianUtils.Core;

namespace Billiard.Areas.Dashboard.Controllers
{
    [Area(AreaConstants.DashboardArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(Title = "مدیریت وبلاگ", UseDefaultRouteUrl = true, Order = 0)]
    public class BlogManagerController : Controller
    {
        private readonly IBlogPostCategoryServices _categoryManager;
        private readonly IBlogPostServices _postManager;

        private readonly IHostingEnvironment _hostingEnvironment;


        private const int DefaultPageSize = 10;


        public BlogManagerController(IBlogPostCategoryServices categoryManager, IBlogPostServices postManager, IHostingEnvironment hostingEnvironment)
        {
            _categoryManager = categoryManager;
            _postManager = postManager;
            _hostingEnvironment = hostingEnvironment;
        }


        [BreadCrumb(Title = "لیست دسته بندی", Order = 1)]
        public IActionResult Index(int page = 1)
        {
            var posts = _postManager.Query().OrderBy(c => c.OrderByDescending(z => z.Id)).SelectPage(page, DefaultPageSize, out int TotalItems).ToList();
            var model = new PagedListViewModel<BlogPost>
            {
                List = posts
            };
            model.Paging.TotalItems = TotalItems;
            model.Paging.CurrentPage = page;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            if (HttpContext.Request.IsAjaxRequest())
            {
                return PartialView("_PostList", model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddPost()
        {
            var model = _categoryManager.Query(x => x.IsDelete == false).OrderBy(c => c.OrderByDescending(c => c.Id)).Select().ToList();

            ViewData["Categories"] = model;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPost(BlogPostViewModel model)
        {
            if (ModelState.IsValid)
            {


                string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "BlogPost");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);


                var file = model.Image;
                if (file != null)
                {
                    if (file.ContentType.ToLower() != "image/jpg" &&
                        file.ContentType.ToLower() != "image/jpeg" &&
                        file.ContentType.ToLower() != "image/pjpeg" &&
                        file.ContentType.ToLower() != "image/gif" &&
                        file.ContentType.ToLower() != "image/x-png" &&
                        file.ContentType.ToLower() != "image/png")
                    {
                        this.ModelState.AddModelError("", $"لطفا فقط عکس انتخاب کنید");

                        var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا فقط عکس انتخاب کنید" };
                        return PartialView("_Message", model: Errormodel);
                    }
                    else
                    {
                        if (file.Length > 0)
                        {
                            long fileSizeInBytes = file.Length;

                            long fileSizeInKB = fileSizeInBytes / 1024;

                            long fileSizeInMB = fileSizeInKB / 1024;
                            if (fileSizeInMB < 10)
                            {
                                var fileName = ContentDispositionHeaderValue.Parse
                                    (file.ContentDisposition).FileName;

                                var fname = Guid.NewGuid().ToString().Replace("-", "") + "-" + fileName.Replace("\"", "");

                                var filePath = Path.Combine(path, fname);

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                }

                                model.ImageFileName = fname;

                            }
                            else
                            {
                                var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا عکس کمتر از 10 مگابایت انتخاب کنید" };
                                return PartialView("_Message", model: Errormodel);
                            }
                        }
                    }
                }


                var Videofile = model.Video;
                if (Videofile != null)
                {
                    if (Videofile.ContentType.ToLower() != "video/mp4"
                        )
                    {
                        this.ModelState.AddModelError("", $"لطفا فقط عکس انتخاب کنید");

                        var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا فقط عکس انتخاب کنید" };
                        return PartialView("_Message", model: Errormodel);
                    }
                    else
                    {
                        if (Videofile.Length > 0)
                        {
                            long fileSizeInBytes = Videofile.Length;

                            long fileSizeInKB = fileSizeInBytes / 1024;

                            long fileSizeInMB = fileSizeInKB / 1024;
                            if (fileSizeInMB < 50)
                            {
                                var fileName = ContentDispositionHeaderValue.Parse
                                    (Videofile.ContentDisposition).FileName;

                                var fname = Guid.NewGuid().ToString().Replace("-", "") + "-" + fileName.Replace("\"", "");

                                var filePath = Path.Combine(path, fname);

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await Videofile.CopyToAsync(fileStream);
                                }

                                model.VideoFileName = fname;

                            }
                            else
                            {
                                var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا عکس کمتر از 10 مگابایت انتخاب کنید" };
                                return PartialView("_Message", model: Errormodel);
                            }
                        }
                    }
                }



                var userId = int.Parse(User.Identity.GetUserId());

                var res = await _postManager.InsertAsync(new BlogPost()
                {

                    ImageFileName = model.ImageFileName,
                    Title = model.Title,
                    Text = model.Text,
                    VideoFileName = model.VideoFileName,
                    BlogPostCategoryId = model.BlogPostCategoryId,
                    SeoDescription = model.SeoDescription,
                    SeoKeyWords = model.SeoKeyWords,
                    IsSpecial = model.IsSpecial,
                    UserId = userId,
                });
                if (res)
                {

                    return RedirectToAction(nameof(Index));

                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult EditPost(int Id)
        {

            if (Id != 0)
            {
                var cat = _categoryManager.Query(x => x.IsDelete == false).OrderBy(c => c.OrderByDescending(c => c.Id)).Select().ToList();

                ViewData["Categories"] = cat;
                var post = _postManager.Query(x => x.Id == Id).Select(x => x);
                if (!post.Any())
                {
                    var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "مطلب مورد نظر شما یافت نشد" };
                    return PartialView("_Message", model: Errormodel);
                }
                var model = post.FirstOrDefault();
                var modelviwmodel = new BlogPostViewModel()
                {
                    ImageFileName = model.ImageFileName,
                    Title = model.Title,
                    Text = model.Text,
                    VideoFileName = model.VideoFileName,
                    BlogPostCategoryId = model.BlogPostCategoryId,
                    SeoDescription = model.SeoDescription,
                    SeoKeyWords = model.SeoKeyWords,
                    IsSpecial = model.IsSpecial,
                    Id = model.Id
                };
                return View(modelviwmodel);
            }
            else
            {
                var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا یک مطلب را برای ویرایش انتخاب کنید" };
                return PartialView("_Message", model: Errormodel);
            }
        }




        [HttpPost]
        public async Task<IActionResult> EditPost(BlogPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var p = _postManager.Query(x => x.Id == model.Id).Select(x => x);
                if (!p.Any())
                {
                    var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "مطلب مورد نظر شما یافت نشد" };
                    return PartialView("_Message", model: Errormodel);
                }

                var _post = p.FirstOrDefault();



                string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "BlogPost");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);


                var file = model.Image;
                if (file != null)
                {
                    if (file.ContentType.ToLower() != "image/jpg" &&
                        file.ContentType.ToLower() != "image/jpeg" &&
                        file.ContentType.ToLower() != "image/pjpeg" &&
                        file.ContentType.ToLower() != "image/gif" &&
                        file.ContentType.ToLower() != "image/x-png" &&
                        file.ContentType.ToLower() != "image/png")
                    {
                        this.ModelState.AddModelError("", $"لطفا فقط عکس انتخاب کنید");

                        var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا فقط عکس انتخاب کنید" };
                        return PartialView("_Message", model: Errormodel);
                    }
                    else
                    {
                        if (file.Length > 0)
                        {
                            long fileSizeInBytes = file.Length;

                            long fileSizeInKB = fileSizeInBytes / 1024;

                            long fileSizeInMB = fileSizeInKB / 1024;
                            if (fileSizeInMB < 10)
                            {
                                var fileName = ContentDispositionHeaderValue.Parse
                                    (file.ContentDisposition).FileName;

                                var fname = Guid.NewGuid().ToString().Replace("-", "") + "-" + fileName.Replace("\"", "");

                                var filePath = Path.Combine(path, fname);

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                }

                                model.ImageFileName = fname;
                                _post.ImageFileName = model.ImageFileName;
                            }
                            else
                            {
                                var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا عکس کمتر از 10 مگابایت انتخاب کنید" };
                                return PartialView("_Message", model: Errormodel);
                            }
                        }
                    }
                }

                var Videofile = model.Video;
                if (Videofile != null)
                {
                    if (Videofile.ContentType.ToLower() != "video/mp4"
                        )
                    {
                        this.ModelState.AddModelError("", $"لطفا فقط عکس انتخاب کنید");

                        var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا فقط عکس انتخاب کنید" };
                        return PartialView("_Message", model: Errormodel);
                    }
                    else
                    {
                        if (Videofile.Length > 0)
                        {
                            long fileSizeInBytes = Videofile.Length;

                            long fileSizeInKB = fileSizeInBytes / 1024;

                            long fileSizeInMB = fileSizeInKB / 1024;
                            if (fileSizeInMB < 50)
                            {
                                var fileName = ContentDispositionHeaderValue.Parse
                                    (Videofile.ContentDisposition).FileName;

                                var fname = Guid.NewGuid().ToString().Replace("-", "") + "-" + fileName.Replace("\"", "");

                                var filePath = Path.Combine(path, fname);

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await Videofile.CopyToAsync(fileStream);
                                }

                                model.VideoFileName = fname;

                            }
                            else
                            {
                                var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا عکس کمتر از 10 مگابایت انتخاب کنید" };
                                return PartialView("_Message", model: Errormodel);
                            }
                        }
                    }
                }



                _post.Title = model.Title;
                _post.Text = model.Text;
                _post.VideoFileName = model.VideoFileName;
                _post.BlogPostCategoryId = model.BlogPostCategoryId;
                _post.SeoDescription = model.SeoDescription;
                _post.SeoKeyWords = model.SeoKeyWords;
                _post.IsSpecial = model.IsSpecial;

                var res = await _postManager.UpdateAsync(_post);
                if (res)
                {

                    return RedirectToAction(nameof(Index));

                }
            }
            return View(model);
        }



        [AjaxOnly]
        public IActionResult RenderDeletePost([FromBody] ModelIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model == null)
            {
                return BadRequest("model is null.");
            }

            var post = _postManager.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (post == null)
            {
                ModelState.AddModelError("", "دسته یافت نشد");
                return PartialView("_DeletePost", model: new BlogPost());
            }
            return PartialView("_DeletePost", model: new BlogPost { Id = post.Id, Title = post.Title });
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(BlogPost model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id == 0)
            {
                return BadRequest("model is null.");
            }

            var post = _postManager.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (post == null)
            {
                ModelState.AddModelError("", "دسته یافت نشد");
            }
            else
            {
                post.IsDelete = true;
                var result = await _postManager.UpdateAsync(post);
                if (result)
                {
                    return Json(new { success = true });
                }
            }
            return PartialView("_DeletePost", model: model);
        }


        [AjaxOnly]
        public IActionResult RenderRecoveryPost([FromBody] ModelIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model == null)
            {
                return BadRequest("model is null.");
            }

            var post = _postManager.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (post == null)
            {
                ModelState.AddModelError("", "مطلب یافت نشد");
                return PartialView("_RecoveryPost", model: new BlogPost());
            }
            return PartialView("_RecoveryPost", model: new BlogPost { Id = post.Id, Title = post.Title });
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoveryPost(BlogPost model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id == 0)
            {
                return BadRequest("model is null.");
            }

            var post = _postManager.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (post == null)
            {
                ModelState.AddModelError("", "مطلب یافت نشد");
            }
            else
            {
                post.IsDelete = false;
                var result = await _postManager.UpdateAsync(post);
                if (result)
                {
                    return Json(new { success = true });
                }
            }
            return PartialView("_RecoveryPost", model: model);
        }













        [BreadCrumb(Title = "لیست دسته بندی", Order = 1)]
        public IActionResult Category(int page = 1, string search = "")
        {
            if (search == null)
            {
                search = "";
            }
            var categories = _categoryManager.Query(x => x.Title.Contains(search) || x.Name.Contains(search)).OrderBy(c => c.OrderByDescending(c => c.Id)).SelectPage(page, DefaultPageSize, out int TotalItems).ToList();
            var model = new PagedListViewModel<BlogPostCategory>
            {
                List = categories
            };

            if (search != "")
            {
                ViewData["search"] = search;
            }
            else
            {
                ViewData["search"] = "";
            }
            model.Paging.TotalItems = TotalItems;
            model.Paging.CurrentPage = page;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            if (HttpContext.Request.IsAjaxRequest())
            {
                return PartialView("_CategoryList", model);
            }
            return View(model);
        }
        public IActionResult ExportExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                worksheet.RightToLeft = true;
                worksheet.SetRightToLeft(true);

                worksheet.Cell(currentRow, 1).Value = "کد سیستم";
                worksheet.Cell(currentRow, 2).Value = "نام دسته";
                worksheet.Cell(currentRow, 3).Value = "عنوان دسته";
                worksheet.Cell(currentRow, 4).Value = "تاریخ درج در سایت";

                var AllCategory = _categoryManager.Query().Select(c => c).ToList();
                foreach (var category in AllCategory)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = category.Id;
                    worksheet.Cell(currentRow, 2).Value = category.Name;
                    worksheet.Cell(currentRow, 3).Value = category.Title;
                    worksheet.Cell(currentRow, 4).Value = category.CreatedDateTime.ToPersianDateTextify();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "category.xlsx");
                }
            }
        }


        [HttpGet]
        [BreadCrumb(Title = "افزودن دسته بندی", Order = 1)]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [BreadCrumb(Title = "افزودن دسته بندی", Order = 1)]
        public async Task<IActionResult> AddCategory(BlogPostCategoryViewModel model)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "BlogCategory");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            var file = model.Image;
            if (file != null)
            {
                if (file.ContentType.ToLower() != "image/jpg" &&
                    file.ContentType.ToLower() != "image/jpeg" &&
                    file.ContentType.ToLower() != "image/pjpeg" &&
                    file.ContentType.ToLower() != "image/gif" &&
                    file.ContentType.ToLower() != "image/x-png" &&
                    file.ContentType.ToLower() != "image/png")
                {
                    this.ModelState.AddModelError("", $"لطفا فقط عکس انتخاب کنید");

                    var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا فقط عکس انتخاب کنید" };
                    return PartialView("_Message", model: Errormodel);
                }
                else
                {
                    if (file.Length > 0)
                    {
                        long fileSizeInBytes = file.Length;

                        long fileSizeInKB = fileSizeInBytes / 1024;

                        long fileSizeInMB = fileSizeInKB / 1024;
                        if (fileSizeInMB < 10)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse
                                (file.ContentDisposition).FileName;

                            var fname = Guid.NewGuid().ToString().Replace("-", "") + "-" + fileName.Replace("\"", "");

                            var filePath = Path.Combine(path, fname);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            model.ImageFileName = fname;

                        }
                        else
                        {
                            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا عکس کمتر از 10 مگابایت انتخاب کنید" };
                            return PartialView("_Message", model: Errormodel);
                        }
                    }
                }
            }
            var res = await _categoryManager.InsertAsync(new BlogPostCategory()
            {
                ImageFileName = model.ImageFileName,
                Title = model.Title,
                Name = model.Name
            });
            if (res)
            {

                return RedirectToAction(nameof(Category));

            }
            return View(model);
        }



        [HttpGet]
        [BreadCrumb(Title = "افزودن دسته بندی", Order = 1)]
        public IActionResult EditCategory(int Id)
        {
            if (Id != 0)
            {
                var category = _categoryManager.Query(x => x.Id == Id).Select(x => x);
                if (!category.Any())
                {
                    var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "دسته مورد نظر شما یافت نشد" };
                    return PartialView("_Message", model: Errormodel);
                }
                var model = category.FirstOrDefault();
                var modelviwmodel = new BlogPostCategoryViewModel()
                {
                    ImageFileName = model.ImageFileName,
                    Title = model.Title,
                    Name = model.Name,
                    Id = model.Id
                };
                return View(modelviwmodel);
            }
            else
            {
                var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا یک دسته را برای ویرایش انتخاب کنید" };
                return PartialView("_Message", model: Errormodel);
            }
        }
        [HttpPost]
        [BreadCrumb(Title = "افزودن دسته بندی", Order = 1)]
        public async Task<IActionResult> EditCategory(BlogPostCategoryViewModel model)
        {

            var category = _categoryManager.Query(x => x.Id == model.Id).Select(x => x);
            if (!category.Any())
            {
                var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "دسته مورد نظر شما یافت نشد" };
                return PartialView("_Message", model: Errormodel);
            }

            var cat = category.FirstOrDefault();

            string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "BlogCategory");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            var file = model.Image;
            if (file != null)
            {
                if (file.ContentType.ToLower() != "image/jpg" &&
                    file.ContentType.ToLower() != "image/jpeg" &&
                    file.ContentType.ToLower() != "image/pjpeg" &&
                    file.ContentType.ToLower() != "image/gif" &&
                    file.ContentType.ToLower() != "image/x-png" &&
                    file.ContentType.ToLower() != "image/png")
                {
                    this.ModelState.AddModelError("", $"لطفا فقط عکس انتخاب کنید");

                    var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا فقط عکس انتخاب کنید" };
                    return PartialView("_Message", model: Errormodel);
                }
                else
                {
                    if (file.Length > 0)
                    {
                        long fileSizeInBytes = file.Length;

                        long fileSizeInKB = fileSizeInBytes / 1024;

                        long fileSizeInMB = fileSizeInKB / 1024;
                        if (fileSizeInMB < 10)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse
                                (file.ContentDisposition).FileName;

                            var fname = Guid.NewGuid().ToString().Replace("-", "") + "-" + fileName.Replace("\"", "");

                            var filePath = Path.Combine(path, fname);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            model.ImageFileName = fname;
                            cat.ImageFileName = model.ImageFileName;

                        }
                        else
                        {
                            var Errormodel = new AlertMessageViewModel() { MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "لطفا عکس کمتر از 10 مگابایت انتخاب کنید" };
                            return PartialView("_Message", model: Errormodel);
                        }
                    }
                }
            }
            cat.Title = model.Title;
            cat.Name = model.Name;

            var res = await _categoryManager.UpdateAsync(cat);
            if (res)
            {

                return RedirectToAction(nameof(Category));

            }
            return View(model);
        }








        [AjaxOnly]
        public IActionResult RenderDeleteCategory([FromBody] ModelIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model == null)
            {
                return BadRequest("model is null.");
            }

            var category = _categoryManager.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (category == null)
            {
                ModelState.AddModelError("", "دسته یافت نشد");
                return PartialView("_DeleteCategory", model: new BlogPostCategory());
            }
            return PartialView("_DeleteCategory", model: new BlogPostCategory { Id = category.Id, Name = category.Name, Title = category.Title });
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(BlogPostCategory model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id == 0)
            {
                return BadRequest("model is null.");
            }

            var category = _categoryManager.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (category == null)
            {
                ModelState.AddModelError("", "دسته یافت نشد");
            }
            else
            {
                category.IsDelete = true;
                var result = await _categoryManager.UpdateAsync(category);
                if (result)
                {
                    return Json(new { success = true });
                }
            }
            return PartialView("_DeleteCategory", model: model);
        }


        [AjaxOnly]
        public IActionResult RenderRecoveryCategory([FromBody] ModelIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model == null)
            {
                return BadRequest("model is null.");
            }

            var category = _categoryManager.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (category == null)
            {
                ModelState.AddModelError("", "دسته یافت نشد");
                return PartialView("_RecoveryCategory", model: new BlogPostCategory());
            }
            return PartialView("_RecoveryCategory", model: new BlogPostCategory { Id = category.Id, Name = category.Name, Title = category.Title });
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoveryCategory(BlogPostCategory model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id == 0)
            {
                return BadRequest("model is null.");
            }

            var category = _categoryManager.Query(c => c.Id == model.Id).Select(c => c).FirstOrDefault();
            if (category == null)
            {
                ModelState.AddModelError("", "دسته یافت نشد");
            }
            else
            {
                category.IsDelete = false;
                var result = await _categoryManager.UpdateAsync(category);
                if (result)
                {
                    return Json(new { success = true });
                }
            }
            return PartialView("_RecoveryCategory", model: model);
        }


    }
}