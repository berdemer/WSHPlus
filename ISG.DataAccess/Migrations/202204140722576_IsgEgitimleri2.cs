namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsgEgitimleri2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IsgEgitimleri", "Tarih", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IsgEgitimleri", "Tarih", c => c.DateTime());
        }
    }
}
