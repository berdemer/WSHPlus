namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ISG_TopluEgitimSablonlari2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ISGTopluEgitimSablonlari", "EgitimJson", unique: true, name: "IDX_ConfigurationId_Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ISGTopluEgitimSablonlari", "IDX_ConfigurationId_Name");
        }
    }
}
