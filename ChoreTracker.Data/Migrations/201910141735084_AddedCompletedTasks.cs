namespace ChoreTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCompletedTasks : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompletedTaskEntity", "UserId", "dbo.ApplicationUser");
            DropForeignKey("dbo.CompletedTaskEntity", "TaskId", "dbo.TaskEntity");
            DropIndex("dbo.CompletedTaskEntity", new[] { "UserId" });
            DropIndex("dbo.CompletedTaskEntity", new[] { "TaskId" });
            DropTable("dbo.CompletedTaskEntity");
        }
    }
}
