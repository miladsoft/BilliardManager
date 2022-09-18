using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Billiard.Entities.AuditableEntity;
using Billiard.Entities.Identity;

namespace Billiard.Entities
{
    public class BlogPost : Entity
    {
        // public BlogPost()
        // {   
        //     BlogPostLikes = new HashSet<BlogPostLike>();
        //     BlogPostComments = new HashSet<BlogPostComment>();
        //     BlogPostViews = new HashSet<BlogPostView>();
        // }    
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string SeoDescription { get; set; }

        public string SeoKeyWords { get; set; }

        public string ImageFileName { get; set; }

        public string  VideoFileName { get; set; }

        public int ViewCount { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }

        public virtual BlogPostCategory BlogPostCategory { get; set; }

        public int BlogPostCategoryId { get; set; }

        public bool IsSpecial { get; set; } = false;

        public bool IsDelete { get; set; } = false;

        public virtual ICollection<BlogPostLike> BlogPostLike { get; set; }

        public virtual ICollection<BlogPostComment> BlogPostComment { get; set; }

        public virtual ICollection<BlogPostView> BlogPostView { get; set; }

        public virtual ICollection<BlogPostRate> BlogPostRate { get; set; } 
    }
}