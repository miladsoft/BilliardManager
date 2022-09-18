using System.Collections.Generic;
using Billiard.Entities.Identity;

namespace Billiard.ViewModels.Identity
{
    public enum UserItemActiveTab
    {
        UserInfo,
        UserAdmin
    }

    public class UserItemViewModel
    {
        public User User { set; get; }
        public bool ShowAdminParts { set; get; }
        public List<Role> Roles { get; set; }
        public UserItemActiveTab ActiveTab { get; set; }
    }
}