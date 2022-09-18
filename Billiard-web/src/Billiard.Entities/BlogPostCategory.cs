using System;
using System.Collections.Generic;
using Billiard.Entities.AuditableEntity;

namespace Billiard.Entities
{
    public class BlogPostCategory : Entity
    {
        public int Id { get; set; }

        public BlogPostCategory()
        {
            BlogPosts = new HashSet<BlogPost>();
        }

        public string Name { get; set; }

        public string Title { get; set; }
      
        public string ImageFileName { get; set; }

        public bool IsDelete { get; set; }=false;

        public virtual ICollection<BlogPost> BlogPosts { get; set; }
    }
}