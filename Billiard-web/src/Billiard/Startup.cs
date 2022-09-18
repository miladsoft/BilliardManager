using Billiard.ViewModels.Identity.Settings;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Billiard.IocConfig;
using DNTCommon.Web.Core;
using Billiard.Common.WebToolkit;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using TradingViewUdfProvider;
using Microsoft.AspNetCore.WebSockets;
using WebSocketManager;
using WebMarkupMin.AspNetCore5;

namespace Billiard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteSettings>(options => Configuration.Bind(options));
            services.Configure<ContentSecurityPolicyConfig>(options => Configuration.GetSection("ContentSecurityPolicyConfig").Bind(options));

            services.AddCustomIdentityServices();

            services.AddMvc(options => options.UseYeKeModelBinder());
            services.AddWebMarkupMin(
                  options =>
                  {
                      options.AllowMinificationInDevelopmentEnvironment = true;
                      options.AllowCompressionInDevelopmentEnvironment = true;
                  })
                  .AddHtmlMinification(
                      options =>
                      {
                          options.MinificationSettings.RemoveRedundantAttributes = true;
                          options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                          options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
                      })
                  .AddHttpCompression();
            services.AddDNTCommonWeb();

            services.AddDNTCaptcha(options =>
            {
                options.UseCookieStorageProvider(SameSiteMode.Strict /* If you are using CORS, set it to `None` */) // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
                                                                                                                    // .UseDistributedCacheStorageProvider() // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.
                .WithEncryptionKey("3213215654")                                                                            // .UseDistributedSerializationProvider()
                .AbsoluteExpiration(minutes: 7)
                .ShowThousandsSeparators(true)
                .WithNoise(pixelsDensity: 25, linesCount: 3)
                .InputNames(
                    new DNTCaptchaComponent
                    {
                        CaptchaHiddenInputName = "Billiard_CaptchaText",
                        CaptchaHiddenTokenName = "Billiard_CaptchaToken",
                        CaptchaInputName = "Billiard_CaptchaInputText",

                    })
                .Identifier("Billiard_Captcha") ;
               
            });
            services.AddCloudscribePagination();

            services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.IgnoreNullValues = true;
                });
            services.AddControllersWithViews();

            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddAntiforgery(options => options.Cookie.Name = "Billiard-X-CSRF");
            services.AddTradingViewProvider<MyTvProvider>();
            services.AddWebSocketManager();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseExceptionHandler("/error/index/500");
            app.UseStatusCodePagesWithReExecute("/error/index/{0}");

            app.UseContentSecurityPolicy();
            // app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;

            app.UseWebSockets();
            app.MapWebSocketManager("/ws", serviceProvider.GetService<ChatMessageHandler>());
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "uploads")),
                RequestPath = new PathString("/file/download")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "areapage",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{page?}");

                endpoints.MapControllerRoute(
                    name: "page",
                    pattern: "{controller=Home}/{action=Index}/{page?}");



                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}/{page?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{page?}");



                endpoints.MapControllerRoute(
                 name: "reffer",
                 pattern: "reffer/{Id?}",
                 defaults: new { controller = "Reffer", action = "Index" });

                endpoints.MapControllerRoute(
                 name: "techtools",
                 pattern: "Dashboard/TechnicalTools/{Id?}",
                 defaults: new { area = "Dashboard" , controller = "TechnicalTools", action = "Index" });

                endpoints.MapControllerRoute(
                 name: "dashReg",
                 pattern: "Dashboard/Register/{Id?}",
                 defaults: new { area = "Dashboard", controller = "Register", action = "Index" });



                endpoints.MapControllerRoute(
                 name: "mobReg",
                 pattern: "Mobile/Register/{Id?}",
                 defaults: new { area = "Mobile", controller = "Register", action = "Index" });
                endpoints.MapRazorPages();
            });
            // Optional TradingView provider settings
            app.UseTradingViewProvider(new TradingViewSettings());





        }
    }
}