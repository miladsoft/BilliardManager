using System;
using System.Collections.Generic;
using System.Linq;
using Billiard.DataLayer.Context;
using Billiard.Entities;
using Billiard.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Billiard.ViewModels;
using DNTPersianUtils.Core;
namespace Billiard.Services
{
    public interface IUserBandCardServices : IService<UserBankCards>
    {
        public Task<bool> CheckCardNumber(string CardNumber, string Cardowner);
        public string GetBankName(string CardNumber);

    }

    public class UserBandCardServices : Service<UserBankCards>, IUserBandCardServices
    {

        public UserBandCardServices(IRepositoryAsync<UserBankCards> repository)
            : base(repository)
        {
        }
        private async Task<string> GetCardNumberInfoApi(string CardNumber)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://api.pod.ir/srv/sc/");
                httpClient.DefaultRequestHeaders.Add("_token_", "70ab4ec437b0474fa6df6428dcd508b9");
                httpClient.DefaultRequestHeaders.Add("_token_issuer_", "1");

                var values = new Dictionary<string, string>
                        {
                            { "scProductId", "34255" },
                            { "cardNumber", CardNumber }
                        };

                var content = new FormUrlEncodedContent(values);

                var response = await httpClient.PostAsync("nzh/doServiceCall", content);

                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
        }

        public async Task<bool> CheckCardNumber(string CardNumber, string Cardowner)
        {
            var json = await GetCardNumberInfoApi(CardNumber);
            var cs = Newtonsoft.Json.JsonConvert.DeserializeObject<CardInfoViewModel>(json);
            if (!cs.hasError)
            {
                var res = cs.result.Replace(" ", "").ApplyCorrectYeKe();
                if (res == Cardowner.Replace(" ", ""))
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

        public string GetBankName(string CardNumber)
        {
            Dictionary<string, string> _BankList = new Dictionary<string, string>();

            _BankList.Add("603799", "بانک ملی ایران");
            _BankList.Add("589210", "بانک سپه");
            _BankList.Add("627648", "بانک توسعه صادرات");
            _BankList.Add("627961", "بانک صنعت و معدن");
            _BankList.Add("603770", "بانک کشاورزی");
            _BankList.Add("628023", "بانک مسکن");
            _BankList.Add("627760", "پست بانک ایران");
            _BankList.Add("502908", "بانک توسعه تعاون");
            _BankList.Add("627412", "بانک اقتصاد نوین");
            _BankList.Add("622106", "بانک پارسیان");
            _BankList.Add("502229", "بانک پاسارگاد");
            _BankList.Add("627488", "بانک کارآفرین");
            _BankList.Add("621986", "بانک سامان");
            _BankList.Add("639346", "بانک سینا");
            _BankList.Add("639607", "بانک سرمایه");
            _BankList.Add("636214", "بانک تات");
            _BankList.Add("502806", "بانک شهر");
            _BankList.Add("502938", "بانک دی");
            _BankList.Add("603769", "بانک صادرات");
            _BankList.Add("610433", "بانک ملت");
            _BankList.Add("627353", "بانک تجارت");
            _BankList.Add("589463", "بانک رفاه");
            _BankList.Add("627381", "بانک انصار");
            _BankList.Add("639370", "بانک مهر اقتصاد");
            _BankList.Add("507677", "موسسه اعتباری نور");
            _BankList.Add("628157", "موسسه اعتباری توسعه");
            _BankList.Add("505801", "موسسه اعتباری کوثر");
            _BankList.Add("606256", "موسسه اعتباری ملل (عسکریه)");
            _BankList.Add("606373", "بانک قرض الحسنه مهرایرانیان");


            var _6charfirstcard = CardNumber.Substring(0, 6);

            var bankname = _BankList.Where(c => c.Key == _6charfirstcard).Select(c => c.Value).FirstOrDefault();
            return bankname;
        }
    }



}