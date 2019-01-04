namespace FacebookV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Likes", "ProfileId", "dbo.Profiles");
            DropIndex("dbo.Comments", new[] { "ProfileId" });
            DropIndex("dbo.Likes", new[] { "ProfileId" });
            RenameColumn(table: "dbo.Photos", name: "ProfileId", newName: "Profile_Id1");
            RenameIndex(table: "dbo.Photos", name: "IX_ProfileId", newName: "IX_Profile_Id1");
            AlterColumn("dbo.Comments", "ProfileId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Likes", "ProfileId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Comments", "ProfileId");
            CreateIndex("dbo.Likes", "ProfileId");
            AddForeignKey("dbo.Comments", "ProfileId", "dbo.Profiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Likes", "ProfileId", "dbo.Profiles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Comments", "ProfileId", "dbo.Profiles");
            DropIndex("dbo.Likes", new[] { "ProfileId" });
            DropIndex("dbo.Comments", new[] { "ProfileId" });
            AlterColumn("dbo.Likes", "ProfileId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Comments", "ProfileId", c => c.String(maxLength: 128));
            RenameIndex(table: "dbo.Photos", name: "IX_Profile_Id1", newName: "IX_ProfileId");
            RenameColumn(table: "dbo.Photos", name: "Profile_Id1", newName: "ProfileId");
            CreateIndex("dbo.Likes", "ProfileId");
            CreateIndex("dbo.Comments", "ProfileId");
            AddForeignKey("dbo.Likes", "ProfileId", "dbo.Profiles", "Id");
            AddForeignKey("dbo.Comments", "ProfileId", "dbo.Profiles", "Id");
        }
    }
}
