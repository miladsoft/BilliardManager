using System;
using System.Collections.Generic;
using System.Linq;
using Billiard.DataLayer.Context;
using Billiard.Entities;
using Billiard.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Billiard.Services
{   
     public interface IBlogPostRateServices : IService<BlogPostRate>
    {
    }

    public class BlogPostRateServices : Service<BlogPostRate>, IBlogPostRateServices
    {
        public BlogPostRateServices(IRepositoryAsync<BlogPostRate> repository)
            : base(repository)
        {
        }
    
    }
}