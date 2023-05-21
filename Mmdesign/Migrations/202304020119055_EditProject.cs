namespace Mmdesign.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditProject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectEditModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Name = c.String(),
                        Seo = c.String(),
                        Keyword = c.String(),
                        Category = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        ShortDesc = c.String(),
                        Investor = c.String(),
                        Address = c.String(),
                        Architect = c.String(),
                        Intro = c.String(),
                        IntroContent = c.String(),
                        Intro1 = c.String(),
                        Intro1Content = c.String(),
                        Intro2 = c.String(),
                        Intro2Content = c.String(),
                        Picture = c.String(),
                        Picture1 = c.String(),
                        Picture2 = c.String(),
                        Picture3 = c.String(),
                        Picture4 = c.String(),
                        CategoryClasses = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProjectEditModels");
        }
    }
}
