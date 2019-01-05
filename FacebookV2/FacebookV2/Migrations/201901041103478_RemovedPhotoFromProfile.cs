namespace FacebookV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedPhotoFromProfile : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Profiles", "ProfilePhotoId", "dbo.Photos");
            DropIndex("dbo.Profiles", new[] { "ProfilePhotoId" });
            AlterColumn("dbo.Profiles", "ProfilePhotoId", c => c.Long(nullable: false));
            CreateIndex("dbo.Profiles", "ProfilePhotoId");
            AddForeignKey("dbo.Profiles", "ProfilePhotoId", "dbo.Photos", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Profiles", "ProfilePhotoId", "dbo.Photos");
            DropIndex("dbo.Profiles", new[] { "ProfilePhotoId" });
            AlterColumn("dbo.Profiles", "ProfilePhotoId", c => c.Long());
            CreateIndex("dbo.Profiles", "ProfilePhotoId");
            AddForeignKey("dbo.Profiles", "ProfilePhotoId", "dbo.Photos", "Id");
        }
    }
}
