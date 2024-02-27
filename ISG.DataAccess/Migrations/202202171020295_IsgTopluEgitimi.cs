namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsgTopluEgitimi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IsgTopluEgitimleri",
                c => new
                    {
                        IsgTopluEgitimi_Id = c.Int(nullable: false, identity: true),
                        belgeTipi = c.Int(nullable: false),
                        isgProfTckNo = c.Long(nullable: false),
                        egitimObjects = c.String(),
                        calisanObjects = c.String(),
                        egitimTarihi = c.DateTime(),
                        egitimYer = c.Int(nullable: false),
                        egitimtur = c.Int(nullable: false),
                        sgkTescilNo = c.String(),
                        egiticiTckNo = c.String(),
                        imzalıDeger = c.String(),
                        firmaKodu = c.String(),
                        sorguNo = c.String(),
                        status = c.Int(nullable: false),
                        Sirket_id = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.IsgTopluEgitimi_Id)
                .ForeignKey("dbo.Sirketler", t => t.Sirket_id, cascadeDelete: true)
                .Index(t => t.Sirket_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IsgTopluEgitimleri", "Sirket_id", "dbo.Sirketler");
            DropIndex("dbo.IsgTopluEgitimleri", new[] { "Sirket_id" });
            DropTable("dbo.IsgTopluEgitimleri");
        }
    }
}
