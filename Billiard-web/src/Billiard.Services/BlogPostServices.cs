using System;
using System.Collections.Generic;
using System.Linq;
using Billiard.DataLayer.Context;
using Billiard.Entities;
using Billiard.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Billiard.Services
{
     public interface IBlogPostServices : IService<BlogPost>
    {
    }

    public class BlogPostServices : Service<BlogPost>, IBlogPostServices
    {
        public BlogPostServices(IRepositoryAsync<BlogPost> repository)
            : base(repository)
        {
        }
    
    }
}
