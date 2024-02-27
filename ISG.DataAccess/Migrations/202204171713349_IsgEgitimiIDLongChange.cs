namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsgEgitimiIDLongChange : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.IsgEgitimleri");
            AlterColumn("dbo.IsgEgitimleri", "Egitim_Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.IsgEgitimleri", "Egitim_Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.IsgEgitimleri");
            AlterColumn("dbo.IsgEgitimleri", "Egitim_Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.IsgEgitimleri", "Egitim_Id");
        }
    }
}
