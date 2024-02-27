namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsgTopluEgitimi_jsonekle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IsgTopluEgitimleri", "EgitimJson", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.IsgTopluEgitimleri", "EgitimJson");
        }
    }
}
