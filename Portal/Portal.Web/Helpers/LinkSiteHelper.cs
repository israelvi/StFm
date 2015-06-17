using System.Collections.Generic;
using System.Web.Mvc;
using DataLayer.Model.Account;
using DataLayer.Repository.Shared;
using LogicLayer.Account;
using Newtonsoft.Json;

namespace Portal.Web.Helpers
{
    public static class LinkSiteHelper
    {
        public static MvcHtmlString LinkSiteByUser(string userName)
        {
            int roleId;
            using (var rep = new AccountRepository())
            {
                roleId = rep.GetRoleIdByUsername(userName);

                if(roleId <= EntityConstants.NO_VALUE)
                    return MvcHtmlString.Create(JsonConvert.SerializeObject(new List<LinkSiteModel>()));
            }

            var links = LinkSiteService.GetLinkSiteByRoleId(roleId);
            return MvcHtmlString.Create(JsonConvert.SerializeObject(links));
        }
    }
}
