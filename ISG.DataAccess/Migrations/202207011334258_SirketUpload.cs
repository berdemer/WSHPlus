namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SirketUpload : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SirketUploadlari",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        Sirket_Id = c.Int(nullable: false),
                        FileName = c.String(),
                        GenericName = c.String(nullable: false),
                        FileLenght = c.Int(nullable: false),
                        MimeType = c.String(nullable: false, maxLength: 100),
                        DosyaTipi = c.String(maxLength: 100, fixedLength: true),
                        DosyaTipiID = c.String(),
                        Konu = c.String(),
                        Hazırlayan = c.String(maxLength: 100, fixedLength: true),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Sirketler", t => t.Sirket_Id, cascadeDelete: true)
                .Index(t => t.Sirket_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SirketUploadlari", "Sirket_Id", "dbo.Sirketler");
            DropIndex("dbo.SirketUploadlari", new[] { "Sirket_Id" });
            DropTable("dbo.SirketUploadlari");
        }
    }
}
