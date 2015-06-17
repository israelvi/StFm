using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Portal.Web.Models;

namespace Portal.Web.Helpers
{
    public static class RolesHelper
    {
        public static ApplicationRole GetRoleByName(string name)
        {
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            var result = roleManager.FindByName(name);
            return result;
        }
    }
}