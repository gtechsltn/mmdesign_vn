namespace Mmdesign.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LongToInt : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Projects");
            AlterColumn("dbo.Projects", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Projects");
            AlterColumn("dbo.Projects", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Projects", "Id");
        }
    }
}
