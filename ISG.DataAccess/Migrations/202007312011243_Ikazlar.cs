namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ikazlar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ikazlar",
                c => new
                    {
                        Ikaz_Id = c.Int(nullable: false, identity: true),
                        IkazText = c.String(),
                        SonucIkazText = c.String(),
                        MuayeneTuru = c.String(maxLength: 50),
                        SonTarih = c.DateTime(),
                        Tarih = c.DateTime(),
                        Personel_Id = c.Int(nullable: false),
                        Durumu = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Ikaz_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ikazlar", "Personel_Id", "dbo.Personel");
            DropIndex("dbo.Ikazlar", new[] { "Personel_Id" });
            DropTable("dbo.Ikazlar");
        }
    }
}
