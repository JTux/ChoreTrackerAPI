namespace ChoreTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompletedTaskEntity",
                c => new
                    {
                        CompletedTaskId = c.Int(nullable: false, identity: true),
                        CompletedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                        IsValid = c.Boolean(nullable: false),
                        TaskId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CompletedTaskId)
                .ForeignKey("dbo.TaskEntity", t => t.TaskId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUser", t => t.UserId)
                .Index(t => t.TaskId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TaskEntity",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        TaskName = c.String(nullable: false),
                        Description = c.String(),
                        CreatedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                        RewardValue = c.Double(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.GroupEntity", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.GroupEntity",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false),
                        OwnerId = c.Guid(nullable: false),
                        GroupInviteCode = c.String(nullable: false),
                        DateFounded = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.GroupMemberEntity",
                c => new
                    {
                        GroupMemberId = c.Int(nullable: false, identity: true),
                        IsAccepted = c.Boolean(nullable: false),
                        IsOfficer = c.Boolean(nullable: false),
                        MemberNickname = c.String(),
                        EarnedPoints = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GroupMemberId)
                .ForeignKey("dbo.GroupEntity", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUser", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.ApplicationUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.IdentityRole", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.RewardEntity",
                c => new
                    {
                        RewardId = c.Int(nullable: false, identity: true),
                        RewardName = c.String(nullable: false),
                        Cost = c.Int(nullable: false),
                        NumberAvailable = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RewardId)
                .ForeignKey("dbo.GroupEntity", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.IdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRole", "IdentityRole_Id", "dbo.IdentityRole");
            DropForeignKey("dbo.CompletedTaskEntity", "UserId", "dbo.ApplicationUser");
            DropForeignKey("dbo.CompletedTaskEntity", "TaskId", "dbo.TaskEntity");
            DropForeignKey("dbo.TaskEntity", "GroupId", "dbo.GroupEntity");
            DropForeignKey("dbo.RewardEntity", "GroupId", "dbo.GroupEntity");
            DropForeignKey("dbo.GroupMemberEntity", "UserId", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserRole", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserLogin", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserClaim", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.GroupMemberEntity", "GroupId", "dbo.GroupEntity");
            DropIndex("dbo.RewardEntity", new[] { "GroupId" });
            DropIndex("dbo.IdentityUserRole", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogin", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaim", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.GroupMemberEntity", new[] { "GroupId" });
            DropIndex("dbo.GroupMemberEntity", new[] { "UserId" });
            DropIndex("dbo.TaskEntity", new[] { "GroupId" });
            DropIndex("dbo.CompletedTaskEntity", new[] { "UserId" });
            DropIndex("dbo.CompletedTaskEntity", new[] { "TaskId" });
            DropTable("dbo.IdentityRole");
            DropTable("dbo.RewardEntity");
            DropTable("dbo.IdentityUserRole");
            DropTable("dbo.IdentityUserLogin");
            DropTable("dbo.IdentityUserClaim");
            DropTable("dbo.ApplicationUser");
            DropTable("dbo.GroupMemberEntity");
            DropTable("dbo.GroupEntity");
            DropTable("dbo.TaskEntity");
            DropTable("dbo.CompletedTaskEntity");
        }
    }
}
