using System.Web.Mvc;
using DataLayer.Model.Account;

namespace Portal.Web.Helpers
{
    public static class UserHelper
    {
        public static MvcHtmlString RoleForUser(string userName)
        {
            using (var rep = new AccountRepository())
            {
                var role = rep.GetRoleDescByUsername(userName);
                if (role != null)
                    return MvcHtmlString.Create(role);
            }

            return MvcHtmlString.Create("ND");
        }
    }
}
