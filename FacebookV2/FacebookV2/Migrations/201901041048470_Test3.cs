namespace FacebookV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "Profile_Id1", "dbo.Profiles");
            DropIndex("dbo.Photos", new[] { "Profile_Id1" });
            RenameColumn(table: "dbo.Photos", name: "Profile_Id1", newName: "ProfileId");
            AlterColumn("dbo.Photos", "ProfileId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Photos", "ProfileId");
            //AddForeignKey("dbo.Photos", "ProfileId", "dbo.Profiles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "ProfileId", "dbo.Profiles");
            DropIndex("dbo.Photos", new[] { "ProfileId" });
            AlterColumn("dbo.Photos", "ProfileId", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.Photos", name: "ProfileId", newName: "Profile_Id1");
            CreateIndex("dbo.Photos", "Profile_Id1");
            AddForeignKey("dbo.Photos", "Profile_Id1", "dbo.Profiles", "Id");
        }
    }
}
