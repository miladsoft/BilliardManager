using System;
using System.Collections.Generic;
using System.Linq;
using Billiard.DataLayer.Context;
using Billiard.Entities;
using Billiard.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Billiard.Services
{   
     public interface IBlogPostCommentServices : IService<BlogPostComment>
    {
    }

    public class BlogPostCommentServices : Service<BlogPostComment>, IBlogPostCommentServices
    {
        public BlogPostCommentServices(IRepositoryAsync<BlogPostComment> repository)
            : base(repository)
        {
        }
    
    }
}