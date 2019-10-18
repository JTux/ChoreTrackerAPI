namespace ChoreTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedClaimedReward : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ClaimedReward", newName: "ClaimedRewardEntity");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ClaimedRewardEntity", newName: "ClaimedReward");
        }
    }
}
