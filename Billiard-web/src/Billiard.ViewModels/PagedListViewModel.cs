using System.Collections.Generic;
using Billiard.Entities.Identity;
using cloudscribe.Web.Pagination;

namespace Billiard.ViewModels
{
    public class PagedListViewModel<T>
    {
        public PagedListViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<T> List { get; set; }
 
        public PaginationSettings Paging { get; set; }
    }
}
