using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DNTCommon.Web.Core;
using System.Text;
using Billiard.Common.WebToolkit;
using Billiard.Services.Contracts.Identity;
using Billiard.Services;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Billiard.ViewModels.Identity;
using Billiard.Entities;
using System.Threading.Tasks;
using Billiard.ViewModels;

namespace Billiard.Controllers
{
    [BreadCrumb(Title = "وبلاگ", UseDefaultRouteUrl = true, Order = 0)]
    public class BlogController : Controller
    {
        private ChatMessageHandler _notificationsMessageHandler { get; set; }

        private readonly IBlogPostCommentServices _commentManager;
        private readonly IBlogPostRateServices _rateManager;
        private readonly IBlogPostCategoryServices _categoryManager;
        private readonly IBlogPostServices _postManager;
        private readonly IHostingEnvironment _hostingEnvironment;


        private const int DefaultPageSize = 3;

        public BlogController(IBlogPostCommentServices commentManager, ChatMessageHandler notificationsMessageHandler, IBlogPostRateServices rateManager, IBlogPostCategoryServices categoryManager, IBlogPostServices postManager, IHostingEnvironment hostingEnvironment)
        {
            _categoryManager = categoryManager;
            _postManager = postManager;
            _hostingEnvironment = hostingEnvironment;
            _rateManager = rateManager;
            _notificationsMessageHandler = notificationsMessageHandler;
            _commentManager = commentManager;
        }



        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {
            var lastSP = _postManager.Query(x => x.IsSpecial == true && x.IsDelete == false).Include(c => c.User).Include(c => c.BlogPostCategory).Select().LastOrDefault();
            ViewData["lastSP"] = lastSP;

            var model = _postManager.Query(x => x.IsSpecial == false && x.IsDelete == false).Include(c => c.User).Include(c => c.BlogPostCategory).Select().OrderByDescending(c => c.Id).Take(3).ToList();
            return View(model);
        }
        [BreadCrumb(Title = "نمایش مطلب", Order = 1)]
        public IActionResult Post(int Id)
        {

            var model = _postManager.Query(x => x.Id == Id && x.IsDelete == false).Include(c => c.User).Include(c => c.BlogPostCategory).Include(c => c.BlogPostCategory).Include(c => c.BlogPostComment).Include(c => c.BlogPostLike).Include(c => c.BlogPostRate).Include(c => c.BlogPostView).Select().FirstOrDefault();

            var MinReadTime = model.Text.ToString().MinReadTime();
            ViewData["MinReadTime"] = MinReadTime;
            var uid = User.Identity.GetUserId<int>();
            var rate = _rateManager.Query(x => x.BlogPostId == Id && x.UserId == uid).Select(c => c.Point);
            if (rate.Any())
            {
                ViewData["Rate"] = rate.FirstOrDefault();
            }
            else
            {
                ViewData["Rate"] = 0;
            }
            var _rate = _rateManager.Query(x => x.BlogPostId == Id).Select(c => c.Point).Sum(); ;

            ViewData["_Rate"] = _rate;

            return View(model);
        }

        [BreadCrumb(Title = "دسته بندی", Order = 1)]
        public IActionResult Category(string Id, int page = 1)
        {
            var category = _categoryManager.Query(c => c.Name == Id).Select(c => c).FirstOrDefault();
            if (category != null)
            {
                ViewData["Category"] = category.Title;
                var posts = _postManager.Query(x => x.IsDelete == false).Include(c => c.User).Include(c => c.BlogPostCategory).Include(c => c.BlogPostCategory).Include(c => c.BlogPostComment).Include(c => c.BlogPostLike).Include(c => c.BlogPostRate).Include(c => c.BlogPostView).Select().Where(x => x.BlogPostCategory.Name == Id);
                var _list = posts.Skip(DefaultPageSize * (page - 1)).Take(DefaultPageSize).ToList();
                var model = new PagedListViewModel<BlogPost>
                {
                    List = _list
                };
                model.Paging.TotalItems = posts.ToList().Count;
                model.Paging.CurrentPage = page;
                model.Paging.ItemsPerPage = DefaultPageSize;
                model.Paging.ShowFirstLast = true;
                return View(model);
            }
            return RedirectToAction(nameof(Index));

        }

        [BreadCrumb(Title = "همه مطالب", Order = 1)]
        public IActionResult AllPosts(int page = 1)
        {

            var posts = _postManager.Query(x => x.IsSpecial != true && x.IsDelete == false).OrderBy(c => c.OrderByDescending(z => z.Id)).Include(c => c.User).Include(c => c.BlogPostCategory).Include(c => c.BlogPostCategory).Include(c => c.BlogPostComment).Include(c => c.BlogPostLike).Include(c => c.BlogPostRate).Include(c => c.BlogPostView).SelectPage(page, DefaultPageSize, out int TotalItems).ToList();
            var model = new PagedListViewModel<BlogPost>
            {
                List = posts
            };
            model.Paging.TotalItems = TotalItems;
            model.Paging.CurrentPage = page;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;
            return View(model);
        }


        [BreadCrumb(Title = "جستجو", Order = 1)]
        public IActionResult Search(string text)
        {
            return View();
        }


        [BreadCrumb(Title = "امتیاز", Order = 1)]
        public async Task<IActionResult> Rate(int rate, int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var uid = User.Identity.GetUserId<int>();

                var q = _rateManager.Query(x => x.BlogPostId == id && x.UserId == uid).Select(c => c);
                if (q.Any())
                {
                    var qq = q.FirstOrDefault();
                    qq.Point = rate;
                    await _rateManager.UpdateAsync(qq);

                }
                else
                {
                    await _rateManager.InsertAsync(new BlogPostRate() { UserId = uid, BlogPostId = id, Point = rate, });
                }

                var point2 = _rateManager.Query(x => x.BlogPostId == id).Select(c => c).ToList();

                var point = _rateManager.Query(x => x.BlogPostId == id).Select(c => c.Point).Sum();

                //send message
                var message = new NotificationViewModel() { Type = "BlogRate", Id = id, Message = point.ToString() };
                var jsonMessage = ClassToJson<NotificationViewModel>.ConverToJson(message);
                await _notificationsMessageHandler.SendMessageToAllAsync(jsonMessage);

                return Content(point.ToString());
            }
            var Errormodel = new AlertMessageViewModel() { IsRefreshPage = "false", MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "برای ثبت امتیاز باید وارد حساب کاربری خود در سناتور شوید" };
            return PartialView("_Message", model: Errormodel);
        }


        [BreadCrumb(Title = "نظر", Order = 1)]
        public IActionResult Comment(string text, int postid)
        {
            if (User.Identity.IsAuthenticated)
            {
                var uid = User.Identity.GetUserId<int>();
                var strUserAgent = Request.Headers["User-Agent"].ToString();
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                _commentManager.InsertAsync(new BlogPostComment()
                {
                    BlogPostId = postid,
                    Text = text,
                    IsConfirmed = Entities.Identity.isConfirmed.waitingforconfirmation,
                    ParentId = 0,
                    UserAgent = strUserAgent,
                    UserIp = remoteIpAddress.ToString(),
                    UserId = uid
                });
                return Ok();
            }
            var Errormodel = new AlertMessageViewModel() { IsRefreshPage = "false", MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "برای ثبت امتیاز باید وارد حساب کاربری خود در سناتور شوید" };
            return PartialView("_Message", model: Errormodel);

        }
        [BreadCrumb(Title = "پاسخ نظر", Order = 1)]
        public IActionResult CommentReplay(string replayText, int Id, int commentId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var uid = User.Identity.GetUserId<int>();
                var strUserAgent = Request.Headers["User-Agent"].ToString();
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                _commentManager.InsertAsync(new BlogPostComment()
                {
                    BlogPostId = Id,
                    Text = replayText,
                    IsConfirmed = Entities.Identity.isConfirmed.waitingforconfirmation,
                    ParentId = commentId,
                    UserAgent = strUserAgent,
                    UserIp = remoteIpAddress.ToString(),
                    UserId = uid
                });
                var SuccessModel = new AlertMessageViewModel() { IsRefreshPage = "true", MessageType = AlertMessageType.success, MessageTitle = "پیام سیستم", MessageText = "پاسخ شما در سایت ثبت شدو پس از تایید نویسنده مطلب در سایت نمایش داده خواهد شد" };
                return PartialView("_Message", model: SuccessModel);

            }
            var Errormodel = new AlertMessageViewModel() { IsRefreshPage = "false", MessageType = AlertMessageType.danger, MessageTitle = "پیام سیستم", MessageText = "برای ثبت امتیاز باید وارد حساب کاربری خود در سناتور شوید" };
            return PartialView("_Message", model: Errormodel);
        }


        [BreadCrumb(Title = "لایک", Order = 1)]
        public IActionResult like(int commentid, int uid)
        {
            return Ok();
        }

    }
}