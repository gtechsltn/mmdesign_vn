namespace Mmdesign.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(),
                        ImageUrl = c.String(),
                        IsActive = c.Boolean(),
                        OrderNo = c.Int(),
                        CreatedOn = c.DateTime(),
                        UpdatedOn = c.DateTime(),
                        CreatedBy = c.Int(),
                        UpdatedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Articles", "CreatedBy", c => c.Int());
            AddColumn("dbo.Articles", "UpdatedBy", c => c.Int());
            AddColumn("dbo.Articles", "OrderNo", c => c.Int());
            AddColumn("dbo.Categories", "CreatedBy", c => c.Int());
            AddColumn("dbo.Categories", "UpdatedBy", c => c.Int());
            AddColumn("dbo.Categories", "OrderNo", c => c.Int());
            AddColumn("dbo.Menus", "CreatedBy", c => c.Int());
            AddColumn("dbo.Menus", "UpdatedBy", c => c.Int());
            AddColumn("dbo.Menus", "OrderNo", c => c.Int());
            AddColumn("dbo.Projects", "OrderNo", c => c.Int());
            AddColumn("dbo.Projects", "CreatedBy", c => c.Int());
            AddColumn("dbo.Projects", "UpdatedBy", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "UpdatedBy");
            DropColumn("dbo.Projects", "CreatedBy");
            DropColumn("dbo.Projects", "OrderNo");
            DropColumn("dbo.Menus", "OrderNo");
            DropColumn("dbo.Menus", "UpdatedBy");
            DropColumn("dbo.Menus", "CreatedBy");
            DropColumn("dbo.Categories", "OrderNo");
            DropColumn("dbo.Categories", "UpdatedBy");
            DropColumn("dbo.Categories", "CreatedBy");
            DropColumn("dbo.Articles", "OrderNo");
            DropColumn("dbo.Articles", "UpdatedBy");
            DropColumn("dbo.Articles", "CreatedBy");
            DropTable("dbo.ProjectImages");
        }
    }
}
