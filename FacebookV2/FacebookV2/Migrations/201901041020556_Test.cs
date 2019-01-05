namespace FacebookV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Profiles", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Groups", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.Comments", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.Likes", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.Messages", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Cities", "CountyId", "dbo.Counties");
            DropForeignKey("dbo.Groups", "AdminId", "dbo.Profiles");
            DropForeignKey("dbo.Messages", "GroupId", "dbo.Groups");
            DropIndex("dbo.Profiles", new[] { "Group_Id" });
            DropIndex("dbo.Groups", new[] { "Profile_Id" });
            CreateTable(
                "dbo.GroupProfiles",
                c => new
                    {
                        GroupId = c.Long(nullable: false),
                        ProfileId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GroupId, t.ProfileId })
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: false)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: false)
                .Index(t => t.GroupId)
                .Index(t => t.ProfileId);
            
            AddForeignKey("dbo.Comments", "PhotoId", "dbo.Photos", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Likes", "PhotoId", "dbo.Photos", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Messages", "ProfileId", "dbo.Profiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Cities", "CountyId", "dbo.Counties", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Groups", "AdminId", "dbo.Profiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Messages", "GroupId", "dbo.Groups", "Id", cascadeDelete: false);
            DropColumn("dbo.Profiles", "Group_Id");
            DropColumn("dbo.Groups", "Profile_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "Profile_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Profiles", "Group_Id", c => c.Long());
            DropForeignKey("dbo.Messages", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "AdminId", "dbo.Profiles");
            DropForeignKey("dbo.Cities", "CountyId", "dbo.Counties");
            DropForeignKey("dbo.Messages", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Likes", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.Comments", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.GroupProfiles", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.GroupProfiles", "GroupId", "dbo.Groups");
            DropIndex("dbo.GroupProfiles", new[] { "ProfileId" });
            DropIndex("dbo.GroupProfiles", new[] { "GroupId" });
            DropTable("dbo.GroupProfiles");
            CreateIndex("dbo.Groups", "Profile_Id");
            CreateIndex("dbo.Profiles", "Group_Id");
            AddForeignKey("dbo.Messages", "GroupId", "dbo.Groups", "Id");
            AddForeignKey("dbo.Groups", "AdminId", "dbo.Profiles", "Id");
            AddForeignKey("dbo.Cities", "CountyId", "dbo.Counties", "Id");
            AddForeignKey("dbo.Messages", "ProfileId", "dbo.Profiles", "Id");
            AddForeignKey("dbo.Likes", "PhotoId", "dbo.Photos", "Id");
            AddForeignKey("dbo.Comments", "PhotoId", "dbo.Photos", "Id");
            AddForeignKey("dbo.Groups", "Profile_Id", "dbo.Profiles", "Id");
            AddForeignKey("dbo.Profiles", "Group_Id", "dbo.Groups", "Id");
        }
    }
}
