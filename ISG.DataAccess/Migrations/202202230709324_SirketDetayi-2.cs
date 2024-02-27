namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SirketDetayi2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SirketDetaylari", "Nacekodu", c => c.String());
            AddColumn("dbo.SirketDetaylari", "TehlikeGrubu", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SirketDetaylari", "TehlikeGrubu");
            DropColumn("dbo.SirketDetaylari", "Nacekodu");
        }
    }
}
