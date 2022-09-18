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
    public interface ITechnicalToolsCoinServices : IService<TechnicalToolsCoin>
    {
    }

    public class TechnicalToolsCoinServices : Service<TechnicalToolsCoin>, ITechnicalToolsCoinServices
    {
        public TechnicalToolsCoinServices(IRepositoryAsync<TechnicalToolsCoin> repository)
            : base(repository)
        {
        }
    }

}