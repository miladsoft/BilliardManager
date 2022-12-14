using System.Security.Claims;
using System.Security.Principal;
using Billiard.DataLayer.Context;
using Billiard.Entities.Identity;
using Billiard.Services.Contracts.Identity;
using Billiard.Services.Identity;
using Billiard.Services.Identity.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Billiard.Services;
using Billiard.Entities;

namespace Billiard.IocConfig
{
    public static class AddCustomServicesExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPrincipal>(provider =>
                provider.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.User ?? ClaimsPrincipal.Current);

            services.AddScoped<ILookupNormalizer, CustomNormalizer>();

            services.AddScoped<ISecurityStampValidator, CustomSecurityStampValidator>();
            services.AddScoped<SecurityStampValidator<User>, CustomSecurityStampValidator>();

            services.AddScoped<IPasswordValidator<User>, CustomPasswordValidator>();
            services.AddScoped<PasswordValidator<User>, CustomPasswordValidator>();

            services.AddScoped<IUserValidator<User>, CustomUserValidator>();
            services.AddScoped<UserValidator<User>, CustomUserValidator>();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationClaimsPrincipalFactory>();
            services.AddScoped<UserClaimsPrincipalFactory<User, Role>, ApplicationClaimsPrincipalFactory>();

            services.AddScoped<IdentityErrorDescriber, CustomIdentityErrorDescriber>();

            services.AddScoped<IApplicationUserStore, ApplicationUserStore>();
            services.AddScoped<UserStore<User, Role, ApplicationDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>, ApplicationUserStore>();

            services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
            services.AddScoped<UserManager<User>, ApplicationUserManager>();

            services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
            services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();

            services.AddScoped<IApplicationSignInManager, ApplicationSignInManager>();
            services.AddScoped<SignInManager<User>, ApplicationSignInManager>();

            services.AddScoped<IApplicationRoleStore, ApplicationRoleStore>();
            services.AddScoped<RoleStore<Role, ApplicationDbContext, int, UserRole, RoleClaim>, ApplicationRoleStore>();

            services.AddScoped<IEmailSender, AuthMessageSender>();
            services.AddScoped<ISmsSender, AuthMessageSender>();


            services.AddScoped<IIdentityDbInitializer, IdentityDbInitializer>();
            services.AddScoped<IUsedPasswordsService, UsedPasswordsService>();
            services.AddScoped<ISiteStatService, SiteStatService>();
            services.AddScoped<IUsersPhotoService, UsersPhotoService>();
            services.AddScoped<ISecurityTrimmingService, SecurityTrimmingService>();
            services.AddScoped<IAppLogItemsService, AppLogItemsService>();


            services.AddScoped<IRepositoryAsync<UserBankCards>, Repository<UserBankCards>>();
            services.AddScoped<IUserBandCardServices, UserBandCardServices>();


            services.AddScoped<IRepositoryAsync<UserBankShebas>, Repository<UserBankShebas>>();
            services.AddScoped<IUserBandShebaServices, UserBandShebaServices>();


            services.AddScoped<IRepositoryAsync<UserLog>, Repository<UserLog>>();
            services.AddScoped<IUserLogServices, UserLogServices>();

            services.AddScoped<IRepositoryAsync<BlogPostCategory>, Repository<BlogPostCategory>>();
            services.AddScoped<IBlogPostCategoryServices, BlogPostCategoryServices>();

            services.AddScoped<IRepositoryAsync<BlogPostView>, Repository<BlogPostView>>();
            services.AddScoped<IBlogPostViewServices, BlogPostViewServices>();

            services.AddScoped<IRepositoryAsync<BlogPostLike>, Repository<BlogPostLike>>();
            services.AddScoped<IBlogPostLikeServices, BlogPostLikeServices>();

            services.AddScoped<IRepositoryAsync<BlogPostView>, Repository<BlogPostView>>();
            services.AddScoped<IBlogPostViewServices, BlogPostViewServices>();

            services.AddScoped<IRepositoryAsync<BlogPostRate>, Repository<BlogPostRate>>();
            services.AddScoped<IBlogPostRateServices, BlogPostRateServices>();

            services.AddScoped<IRepositoryAsync<BlogPost>, Repository<BlogPost>>();
            services.AddScoped<IBlogPostServices, BlogPostServices>();

            services.AddScoped<IRepositoryAsync<BlogPostComment>, Repository<BlogPostComment>>();
            services.AddScoped<IBlogPostCommentServices, BlogPostCommentServices>();

            services.AddScoped<IRepositoryAsync<BilliardConsulting>, Repository<BilliardConsulting>>();
            services.AddScoped<IBilliardConsultingServices, BilliardConsultingServices>();
            
            services.AddScoped<IRepositoryAsync<WalletBilliard>, Repository<WalletBilliard>>();
            services.AddScoped<IWalletBilliardServices, WalletBilliardServices>();

            services.AddScoped<IRepositoryAsync<TechnicalToolsCoin>, Repository<TechnicalToolsCoin>>();
            services.AddScoped<ITechnicalToolsCoinServices, TechnicalToolsCoinServices>();


            return services;
        }
    }
}