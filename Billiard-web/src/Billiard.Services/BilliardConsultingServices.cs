using System;
using System.Collections.Generic;
using System.Linq;
using Billiard.DataLayer.Context;
using Billiard.Entities;
using Billiard.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Billiard.Services
{
     public interface IBilliardConsultingServices : IService<BilliardConsulting>
    {
    }

    public class BilliardConsultingServices : Service<BilliardConsulting>, IBilliardConsultingServices
    {
        public BilliardConsultingServices(IRepositoryAsync<BilliardConsulting> repository)
            : base(repository)
        {
        }
    
    }
}
