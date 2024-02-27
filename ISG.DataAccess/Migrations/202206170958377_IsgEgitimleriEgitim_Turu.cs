namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsgEgitimleriEgitim_Turu : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IsgEgitimleri", "Egitim_Turu", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IsgEgitimleri", "Egitim_Turu", c => c.String(nullable: false, maxLength: 75, fixedLength: true));
        }
    }
}
