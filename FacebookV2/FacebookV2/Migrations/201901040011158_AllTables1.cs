namespace FacebookV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllTables1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ProfileId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profiles", t => t.ProfileId)
                .Index(t => t.ProfileId);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AlbumId = c.Long(),
                        Caption = c.String(maxLength: 300),
                        Content = c.Binary(nullable: false),
                        ProfileId = c.String(maxLength: 128),
                        Profile_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id)
                .ForeignKey("dbo.Profiles", t => t.ProfileId)
                .Index(t => t.AlbumId)
                .Index(t => t.ProfileId)
                .Index(t => t.Profile_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Body = c.String(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Status = c.Byte(nullable: false),
                        PhotoId = c.Long(nullable: false),
                        ProfileId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.PhotoId)
                .ForeignKey("dbo.Profiles", t => t.ProfileId)
                .Index(t => t.PhotoId)
                .Index(t => t.ProfileId);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        IsMale = c.Boolean(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
                        IsDeletedByAdmin = c.Boolean(nullable: false),
                        Birthday = c.DateTime(),
                        CityId = c.Int(),
                        CountyId = c.Int(),
                        ProfilePhotoId = c.Long(),
                        Group_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Counties", t => t.CountyId)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .ForeignKey("dbo.Photos", t => t.ProfilePhotoId)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.CityId)
                .Index(t => t.CountyId)
                .Index(t => t.ProfilePhotoId)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountyId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Counties", t => t.CountyId)
                .Index(t => t.CountyId);
            
            CreateTable(
                "dbo.Counties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        ShortName = c.String(nullable: false, maxLength: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FriendRequests",
                c => new
                    {
                        RequesterProfileId = c.String(nullable: false, maxLength: 128),
                        RequestedProfileId = c.String(nullable: false, maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequesterProfileId, t.RequestedProfileId })
                .ForeignKey("dbo.Profiles", t => t.RequestedProfileId)
                .ForeignKey("dbo.Profiles", t => t.RequesterProfileId)
                .Index(t => t.RequesterProfileId)
                .Index(t => t.RequestedProfileId);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        FirstUserId = c.String(nullable: false, maxLength: 128),
                        SecondUserId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.FirstUserId, t.SecondUserId })
                .ForeignKey("dbo.Profiles", t => t.SecondUserId)
                .ForeignKey("dbo.Profiles", t => t.FirstUserId)
                .Index(t => t.FirstUserId)
                .Index(t => t.SecondUserId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AdminId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 150),
                        Description = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Profile_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profiles", t => t.AdminId)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id)
                .Index(t => t.AdminId)
                .Index(t => t.Profile_Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ProfileId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Long(nullable: false),
                        Body = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfileId, t.GroupId })
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.Profiles", t => t.ProfileId)
                .Index(t => t.ProfileId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreatedOn = c.DateTime(nullable: false),
                        PhotoId = c.Long(nullable: false),
                        ProfileId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.PhotoId)
                .ForeignKey("dbo.Profiles", t => t.ProfileId)
                .Index(t => t.PhotoId)
                .Index(t => t.ProfileId);
            
            //CreateTable(
            //    "dbo.AspNetUsers",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            CreatedDate = c.DateTime(nullable: false),
            //            Email = c.String(maxLength: 256),
            //            EmailConfirmed = c.Boolean(nullable: false),
            //            PasswordHash = c.String(),
            //            SecurityStamp = c.String(),
            //            PhoneNumber = c.String(),
            //            PhoneNumberConfirmed = c.Boolean(nullable: false),
            //            TwoFactorEnabled = c.Boolean(nullable: false),
            //            LockoutEndDateUtc = c.DateTime(),
            //            LockoutEnabled = c.Boolean(nullable: false),
            //            AccessFailedCount = c.Int(nullable: false),
            //            UserName = c.String(nullable: false, maxLength: 256),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            //CreateTable(
            //    "dbo.AspNetUserClaims",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            ClaimType = c.String(),
            //            ClaimValue = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.AspNetUserLogins",
            //    c => new
            //        {
            //            LoginProvider = c.String(nullable: false, maxLength: 128),
            //            ProviderKey = c.String(nullable: false, maxLength: 128),
            //            UserId = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.AspNetUserRoles",
            //    c => new
            //        {
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            RoleId = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.UserId, t.RoleId })
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId)
            //    .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
            //    .Index(t => t.UserId)
            //    .Index(t => t.RoleId);
            
            //CreateTable(
            //    "dbo.AspNetRoles",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(nullable: false, maxLength: 256),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Photos", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Profiles", "Id", "dbo.AspNetUsers");
            //DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            //DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            //DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Profiles", "ProfilePhotoId", "dbo.Photos");
            DropForeignKey("dbo.Photos", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.Likes", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Likes", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.Groups", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.Profiles", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Messages", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Messages", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "AdminId", "dbo.Profiles");
            DropForeignKey("dbo.Friends", "FirstUserId", "dbo.Profiles");
            DropForeignKey("dbo.Friends", "SecondUserId", "dbo.Profiles");
            DropForeignKey("dbo.FriendRequests", "RequesterProfileId", "dbo.Profiles");
            DropForeignKey("dbo.FriendRequests", "RequestedProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Comments", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Profiles", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Profiles", "CountyId", "dbo.Counties");
            DropForeignKey("dbo.Cities", "CountyId", "dbo.Counties");
            DropForeignKey("dbo.Albums", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Comments", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.Photos", "AlbumId", "dbo.Albums");
            //DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            //DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            //DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            //DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            //DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            //DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Likes", new[] { "ProfileId" });
            DropIndex("dbo.Likes", new[] { "PhotoId" });
            DropIndex("dbo.Messages", new[] { "GroupId" });
            DropIndex("dbo.Messages", new[] { "ProfileId" });
            DropIndex("dbo.Groups", new[] { "Profile_Id" });
            DropIndex("dbo.Groups", new[] { "AdminId" });
            DropIndex("dbo.Friends", new[] { "SecondUserId" });
            DropIndex("dbo.Friends", new[] { "FirstUserId" });
            DropIndex("dbo.FriendRequests", new[] { "RequestedProfileId" });
            DropIndex("dbo.FriendRequests", new[] { "RequesterProfileId" });
            DropIndex("dbo.Cities", new[] { "CountyId" });
            DropIndex("dbo.Profiles", new[] { "Group_Id" });
            DropIndex("dbo.Profiles", new[] { "ProfilePhotoId" });
            DropIndex("dbo.Profiles", new[] { "CountyId" });
            DropIndex("dbo.Profiles", new[] { "CityId" });
            DropIndex("dbo.Profiles", new[] { "Id" });
            DropIndex("dbo.Comments", new[] { "ProfileId" });
            DropIndex("dbo.Comments", new[] { "PhotoId" });
            DropIndex("dbo.Photos", new[] { "Profile_Id" });
            DropIndex("dbo.Photos", new[] { "ProfileId" });
            DropIndex("dbo.Photos", new[] { "AlbumId" });
            DropIndex("dbo.Albums", new[] { "ProfileId" });
            //DropTable("dbo.AspNetRoles");
            //DropTable("dbo.AspNetUserRoles");
            //DropTable("dbo.AspNetUserLogins");
            //DropTable("dbo.AspNetUserClaims");
            //DropTable("dbo.AspNetUsers");
            DropTable("dbo.Likes");
            DropTable("dbo.Messages");
            DropTable("dbo.Groups");
            DropTable("dbo.Friends");
            DropTable("dbo.FriendRequests");
            DropTable("dbo.Counties");
            DropTable("dbo.Cities");
            DropTable("dbo.Profiles");
            DropTable("dbo.Comments");
            DropTable("dbo.Photos");
            DropTable("dbo.Albums");
        }
    }
}
