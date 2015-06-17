using System.Collections.Generic;

namespace DataLayer.Model.Account
{
    public class LinkSiteModel
    {
        public int NodeId { get; set; }
        public string NodeName { get; set; }
        public int RoleId { get; set; }
        public int Permision { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public int NodeType { get; set; }
        public string Icon { get; set; }
        public bool IsLeaf { get; set; }
        public List<LinkSiteModel> Children { get; set; }

        public string NodeNameRef
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NodeName))
                    return "nd";

                return NodeName.Replace(" ", string.Empty);
            }
        }
    }
}