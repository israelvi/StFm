using System.Collections.Generic;
using System.Linq;
using DataLayer.Model.Account;
using DataLayer.Repository.Shared;

namespace DataLayer.Repository.Account
{
    public class SiteRepository : BaseRepository
    {
        public IList<LinkSiteModel> GetLinkByRole(int roleId)
        {
            return
                DbConn.AspNetNodeSiteMap.Join(DbConn.AspNetPermissionNodePerRole, e => e.Id, i => i.NodeId,
                    (node, perm) => new
                    {
                        NodeId = node.Id,
                        node.NodeName,
                        perm.RoleId,
                        perm.Permision,
                        node.Url,
                        ParentId = node.Parent,
                        node.NodeType,
                        node.Icon,
                        node.IsLeaf
                    }).Where(e => e.RoleId == roleId && e.Permision > 0).Select(e => new LinkSiteModel
                    {
                        NodeId = e.NodeId,
                        NodeName = e.NodeName,
                        RoleId = e.RoleId,
                        Permision = e.Permision,
                        Url = e.Url,
                        ParentId = e.ParentId,
                        NodeType  = e.NodeType,
                        Icon = e.Icon,
                        IsLeaf = e.IsLeaf
                    }).ToList();
        }
    }
}