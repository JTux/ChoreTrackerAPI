namespace ChoreTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTasks : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TaskEntity", "IsComplete");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskEntity", "IsComplete", c => c.Boolean(nullable: false));
        }
    }
}
