namespace ProjectScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Format : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Notes", c => c.String(maxLength: 40));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Notes", c => c.String(maxLength: 50));
        }
    }
}
