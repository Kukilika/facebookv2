namespace FacebookV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeNullabelProfilePhoto : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Profiles", "ProfilePhotoId", "dbo.Photos");
            DropForeignKey("dbo.Photos", "Profile_Id", "dbo.Profiles");
            DropIndex("dbo.Photos", new[] { "ProfileId" });
            DropIndex("dbo.Photos", new[] { "Profile_Id" });
            DropIndex("dbo.Profiles", new[] { "ProfilePhotoId" });
            DropColumn("dbo.Photos", "ProfileId");
            RenameColumn(table: "dbo.Photos", name: "Profile_Id", newName: "ProfileId");
            AlterColumn("dbo.Photos", "ProfileId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Profiles", "ProfilePhotoId", c => c.Long());
            CreateIndex("dbo.Photos", "ProfileId");
            AddForeignKey("dbo.Photos", "ProfileId", "dbo.Profiles", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "ProfileId", "dbo.Profiles");
            DropIndex("dbo.Photos", new[] { "ProfileId" });
            AlterColumn("dbo.Profiles", "ProfilePhotoId", c => c.Long(nullable: false));
            AlterColumn("dbo.Photos", "ProfileId", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.Photos", name: "ProfileId", newName: "Profile_Id");
            AddColumn("dbo.Photos", "ProfileId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Profiles", "ProfilePhotoId");
            CreateIndex("dbo.Photos", "Profile_Id");
            CreateIndex("dbo.Photos", "ProfileId");
            AddForeignKey("dbo.Photos", "Profile_Id", "dbo.Profiles", "Id");
            AddForeignKey("dbo.Profiles", "ProfilePhotoId", "dbo.Photos", "Id", cascadeDelete: true);
        }
    }
}
