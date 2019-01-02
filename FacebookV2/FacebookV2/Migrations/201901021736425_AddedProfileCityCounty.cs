namespace FacebookV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProfileCityCounty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountyId = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Counties", t => t.CountyId, cascadeDelete: true)
                .Index(t => t.CountyId);
            
            CreateTable(
                "dbo.Counties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        ShortName = c.String(maxLength: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        IsMale = c.Boolean(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
                        Birthday = c.DateTime(),
                        CityId = c.Int(),
                        CountyId = c.Int(),
                        ProfilePhotoId = c.Long(),
                        IsDeletedByAdmin = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Counties", t => t.CountyId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CityId)
                .Index(t => t.CountyId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AspNetUsers", "County_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "City_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "County_Id");
            CreateIndex("dbo.AspNetUsers", "City_Id");
            AddForeignKey("dbo.AspNetUsers", "County_Id", "dbo.Counties", "Id");
            AddForeignKey("dbo.AspNetUsers", "City_Id", "dbo.Cities", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Profiles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Profiles", "CountyId", "dbo.Counties");
            DropForeignKey("dbo.Profiles", "CityId", "dbo.Cities");
            DropForeignKey("dbo.AspNetUsers", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.AspNetUsers", "County_Id", "dbo.Counties");
            DropForeignKey("dbo.Cities", "CountyId", "dbo.Counties");
            DropIndex("dbo.Profiles", new[] { "UserId" });
            DropIndex("dbo.Profiles", new[] { "CountyId" });
            DropIndex("dbo.Profiles", new[] { "CityId" });
            DropIndex("dbo.AspNetUsers", new[] { "City_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "County_Id" });
            DropIndex("dbo.Cities", new[] { "CountyId" });
            DropColumn("dbo.AspNetUsers", "City_Id");
            DropColumn("dbo.AspNetUsers", "County_Id");
            DropTable("dbo.Profiles");
            DropTable("dbo.Counties");
            DropTable("dbo.Cities");
        }
    }
}
