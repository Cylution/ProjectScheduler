namespace ProjectScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Notes");
        }
    }
}
