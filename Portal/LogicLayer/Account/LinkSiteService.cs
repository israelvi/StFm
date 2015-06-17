using System.Collections.Generic;
using System.Linq;
using DataLayer.Model.Account;
using DataLayer.Repository.Account;

namespace LogicLayer.Account
{
    public class LinkSiteService
    {
        public static List<LinkSiteModel> GetLinkSiteByRoleId(int roleId)
        {
            IList<LinkSiteModel> lstLinks;

            using (var repository = new SiteRepository())
            {
                lstLinks = repository.GetLinkByRole(roleId);
            }

            var lstParentLink = lstLinks.Where(e => e.ParentId == null).OrderBy(e => e.NodeName).ToList();
            var lstChildren = lstLinks.Where(e => e.ParentId != null).OrderBy(e => e.NodeName).ToList();

            foreach (var parent in lstParentLink)
            {
                var parentInt = parent;
                parent.Children = lstChildren.Where(e => e.ParentId == parentInt.NodeId).ToList();
            }

            return lstParentLink;
        }
    }
}
