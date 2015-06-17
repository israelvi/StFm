using System.Data.Entity.Migrations;

namespace Portal.Web.Migrations
{
    public partial class UsuarioObsoleto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsObsolete", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsObsolete");
        }
    }
}
