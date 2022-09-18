using System;
using System.Collections.Generic;
using System.Linq;
using Billiard.DataLayer.Context;
using Billiard.Entities;
using Billiard.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Billiard.Services
{   
     public interface IBlogPostCategoryServices : IService<BlogPostCategory>
    {
    }

    public class BlogPostCategoryServices : Service<BlogPostCategory>, IBlogPostCategoryServices
    {
        public BlogPostCategoryServices(IRepositoryAsync<BlogPostCategory> repository)
            : base(repository)
        {
        }
    
    }
}