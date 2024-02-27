namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SirketDetayi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SirketDetaylari", "SirketYetkilisi", c => c.String());
            AddColumn("dbo.SirketDetaylari", "SirketYetkilisiTcNo", c => c.String());
            AddColumn("dbo.SirketDetaylari", "WebUrl", c => c.String());
            AddColumn("dbo.SirketDetaylari", "WebUrlApi", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SirketDetaylari", "WebUrlApi");
            DropColumn("dbo.SirketDetaylari", "WebUrl");
            DropColumn("dbo.SirketDetaylari", "SirketYetkilisiTcNo");
            DropColumn("dbo.SirketDetaylari", "SirketYetkilisi");
        }
    }
}
