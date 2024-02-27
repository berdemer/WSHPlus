namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsgToplu_IsgEgitimiEsle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IsgEgitimleri", "DersTipi", c => c.Int(nullable: false));
            AddColumn("dbo.IsgEgitimleri", "IsgTopluEgitimi_Id", c => c.Int(nullable: false));
            AddColumn("dbo.IsgTopluEgitimleri", "nitelikDurumu", c => c.String());
            CreateIndex("dbo.IsgEgitimleri", "IsgTopluEgitimi_Id");
            AddForeignKey("dbo.IsgEgitimleri", "IsgTopluEgitimi_Id", "dbo.IsgTopluEgitimleri", "IsgTopluEgitimi_Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IsgEgitimleri", "IsgTopluEgitimi_Id", "dbo.IsgTopluEgitimleri");
            DropIndex("dbo.IsgEgitimleri", new[] { "IsgTopluEgitimi_Id" });
            DropColumn("dbo.IsgTopluEgitimleri", "nitelikDurumu");
            DropColumn("dbo.IsgEgitimleri", "IsgTopluEgitimi_Id");
            DropColumn("dbo.IsgEgitimleri", "DersTipi");
        }
    }
}
