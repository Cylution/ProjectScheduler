namespace ProjectScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sortAdded : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Notes", c => c.String(maxLength: 400));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Notes", c => c.String(maxLength: 40));
        }
    }
}
