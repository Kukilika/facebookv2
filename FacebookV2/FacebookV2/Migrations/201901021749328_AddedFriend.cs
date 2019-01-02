namespace FacebookV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFriend : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        SenderId = c.String(nullable: false, maxLength: 128),
                        ReceiverId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.SenderId, t.ReceiverId })
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId, cascadeDelete: true)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friends", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friends", "ReceiverId", "dbo.AspNetUsers");
            DropIndex("dbo.Friends", new[] { "ReceiverId" });
            DropIndex("dbo.Friends", new[] { "SenderId" });
            DropTable("dbo.Friends");
        }
    }
}
