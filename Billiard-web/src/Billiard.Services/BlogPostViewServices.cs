using System;
using System.Collections.Generic;
using System.Linq;
using Billiard.DataLayer.Context;
using Billiard.Entities;
using Billiard.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Billiard.Services
{   
     public interface IBlogPostViewServices : IService<BlogPostView>
    {
    }

    public class BlogPostViewServices : Service<BlogPostView>, IBlogPostViewServices
    {
        public BlogPostViewServices(IRepositoryAsync<BlogPostView> repository)
            : base(repository)
        {
        }
    
    }
}