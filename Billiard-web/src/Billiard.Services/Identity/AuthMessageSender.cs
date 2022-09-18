using Billiard.Services.Contracts.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Billiard.ViewModels.Identity.Settings;
using DNTCommon.Web.Core;
using System;
using SmsIrRestfulNetCore;
using System.Collections.Generic;

namespace Billiard.Services.Identity
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2551
    /// And http://www.dotnettips.info/post/2564
    /// </summary>
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly IOptionsSnapshot<SiteSettings> _smtpConfig;
        private readonly IWebMailService _webMailService;

        public AuthMessageSender(
            IOptionsSnapshot<SiteSettings> smtpConfig,
            IWebMailService webMailService)
        {
            _smtpConfig = smtpConfig ?? throw new ArgumentNullException(nameof(_smtpConfig));
            _webMailService = webMailService ?? throw new ArgumentNullException(nameof(webMailService));
        }

        public Task SendEmailAsync<T>(string email, string subject, string viewNameOrPath, T model)
        {
            return _webMailService.SendEmailAsync(
                _smtpConfig.Value.Smtp,
                new[] { new MailAddress { ToName = "", ToAddress = email } },
                subject,
                viewNameOrPath,
                model
            );
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return _webMailService.SendEmailAsync(
                _smtpConfig.Value.Smtp,
                new[] { new MailAddress { ToName = "", ToAddress = email } },
                subject,
                message
            );
        }
        public Task SendSmsAsync(int TemplateId,string number, string code)
        {
            var token = new Token().GetToken("ad2c8740c386972aa4ba8740", "9516239516232020Mr");
            var ultraFastSend = new UltraFastSend()
            {
                Mobile = long.Parse(number),
                TemplateId = TemplateId,
                ParameterArray = new List<UltraFastParameters>()
                    {
                        new UltraFastParameters()
                        {
                            Parameter = "VerificationCode" , ParameterValue = code
                        }
                    }.ToArray()
            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            if (ultraFastSendRespone.IsSuccessful)
            {
                return Task.FromResult(0);
            }
            else
            {
                return Task.FromResult(1);
            }
        }

    }
}