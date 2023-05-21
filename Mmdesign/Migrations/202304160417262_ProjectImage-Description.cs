namespace Mmdesign.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectImageDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectImages", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectImages", "Description");
        }
    }
}
