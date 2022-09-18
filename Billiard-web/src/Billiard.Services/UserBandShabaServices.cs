using System;
using System.Collections.Generic;
using System.Linq;
using Billiard.DataLayer.Context;
using Billiard.Entities;
using Billiard.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Billiard.ViewModels;
using DNTPersianUtils.Core;
using System.Net.Http;

namespace Billiard.Services
{
    public interface IUserBandShebaServices : IService<UserBankShebas>
    {
        public Task<bool> CheckShebaNumber(string ShebaNumber, string Shebaowner);

    }

    public class UserBandShebaServices : Service<UserBankShebas>, IUserBandShebaServices
    {

        public UserBandShebaServices(IRepositoryAsync<UserBankShebas> repository)
            : base(repository)
        {
        }

        private async Task<string> GetShebaNumberInfoApi(string ShebaNumber)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://api.pod.ir/srv/sc/");
                httpClient.DefaultRequestHeaders.Add("_token_", "70ab4ec437b0474fa6df6428dcd508b9");
                httpClient.DefaultRequestHeaders.Add("_token_issuer_", "1");

                var values = new Dictionary<string, string>
                        {
                            { "scProductId", "34254" },
                            { "sheba", ShebaNumber }
                        };

                var content = new FormUrlEncodedContent(values);

                var response = await httpClient.PostAsync("nzh/doServiceCall", content);

                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
        }

        public async Task<bool> CheckShebaNumber(string ShebaNumber, string Shebaowner)
        {
            var json = await GetShebaNumberInfoApi(ShebaNumber);
            var cs = Newtonsoft.Json.JsonConvert.DeserializeObject<ShebaInfoViewModel>(json);
            if (!cs.hasError)
            {
                var res = (cs.result.owners.FirstOrDefault().firstName+cs.result.owners.FirstOrDefault().lastName).Replace(" ", "").ApplyCorrectYeKe();
                if (res == Shebaowner.Replace(" ", ""))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }


    }

}