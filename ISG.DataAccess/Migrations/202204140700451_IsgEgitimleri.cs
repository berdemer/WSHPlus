namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsgEgitimleri : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.IsgEgitimleri", "Guncelleme_Suresi_Ay");
            DropColumn("dbo.IsgEgitimleri", "GuncellenecekTarih");
            DropColumn("dbo.IsgEgitimleri", "Tamamlandi");
            DropColumn("dbo.IsgEgitimleri", "Aciklama");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IsgEgitimleri", "Aciklama", c => c.String());
            AddColumn("dbo.IsgEgitimleri", "Tamamlandi", c => c.Boolean());
            AddColumn("dbo.IsgEgitimleri", "GuncellenecekTarih", c => c.DateTime());
            AddColumn("dbo.IsgEgitimleri", "Guncelleme_Suresi_Ay", c => c.Int());
        }
    }
}
