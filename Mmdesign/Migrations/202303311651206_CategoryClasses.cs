namespace Mmdesign.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryClasses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "CategoryClasses", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "CategoryClasses");
        }
    }
}
