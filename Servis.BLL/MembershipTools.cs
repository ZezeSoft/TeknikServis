using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Servis.DAL;
using Servis.Entity.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.BLL.Account
{
    public class MembershipTools
    {
        public static UserStore<ApplicationUser> NewUserStore()
        {
            return new UserStore<ApplicationUser>(new ServisContex());
        }
        public static UserManager<ApplicationUser> NewUserManager()
        {
            var userStore = NewUserStore();
            return new UserManager<ApplicationUser>(userStore);
        }
        public static RoleStore<ApplicationRole> NewRoleStore()
        {
            return new RoleStore<ApplicationRole>(new ServisContex());
        }
        public static RoleManager<ApplicationRole> NewRoleManager()
        {
            return new RoleManager<ApplicationRole>(NewRoleStore());
        }
        public static string GetUserName(string id){
            var userManager = NewUserManager();
            var user = userManager.FindById(id);
            if (user != null)
                return user.Name;
            return "Null";
        }
    }
}
