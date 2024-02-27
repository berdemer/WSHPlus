namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ISG_TopluEgitimSablonlari : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ISGTopluEgitimSablonlari",
                c => new
                    {
                        ISG_TopluEgitimSablonlariId = c.Int(nullable: false, identity: true),
                        EgitimJson = c.String(maxLength: 50),
                        EgitimJson1 = c.String(),
                        Status = c.Boolean(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ISG_TopluEgitimSablonlariId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ISGTopluEgitimSablonlari");
        }
    }
}
