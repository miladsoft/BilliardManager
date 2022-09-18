using System;
using System.Collections.Generic;
using System.Linq;
using Billiard.DataLayer.Context;
using Billiard.Entities;
using Billiard.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Billiard.Services
{   
     public interface IBlogPostLikeServices : IService<BlogPostLike>
    {
    }

    public class BlogPostLikeServices : Service<BlogPostLike>, IBlogPostLikeServices
    {
        public BlogPostLikeServices(IRepositoryAsync<BlogPostLike> repository)
            : base(repository)
        {
        }
    
    }
}