namespace Mmdesign.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Menu_Article_Category : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(),
                        Content = c.String(),
                        Title = c.String(),
                        Created = c.DateTime(nullable: false),
                        Published = c.DateTime(),
                        AuthorId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentId = c.Int(nullable: false),
                        Controller = c.String(),
                        Action = c.String(),
                        Slug = c.String(),
                        Params = c.String(),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsHorizontal = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Categories", "ParentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "ParentId");
            DropTable("dbo.Menus");
            DropTable("dbo.Articles");
        }
    }
}
