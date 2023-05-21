using Mmdesign.Models;
using Mmdesign.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Mmdesign
{
    public class MySeedData : DropCreateDatabaseIfModelChanges<MyContextDb>
    {
        protected override void Seed(MyContextDb context)
        {
            GetMenus().ForEach(c => context.Menus.Add(c));
            GetCategories().ForEach(c => context.Categories.Add(c));
            GetProjects().ForEach(g => context.Projects.Add(g));

            context.Commit();
        }

        private static List<Menu> GetMenus()
        {
            return new List<Menu>
            {
                new Menu() {
                    Id = 1,
                    ParentId = 0,
                    Name = "Trang chủ",
                    Title = "Trang chủ",
                    Action = "Index",
                    Controller = "Home",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    IsActive = true,
                    IsHorizontal = true,
                    Slug = "/Home/Index",
                    Params = "",
                },
                new Menu() {
                    Id = 2,
                    ParentId = 0,
                    Title = "Dự án",
                    Name = "Dự án",
                    Action = nameof(Project),
                    Controller = "Home",
                    Slug = "/Home/Project",
                    Params = "",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    IsActive = true,
                    IsHorizontal = true,
                },
                new Menu() {
                    Id = 3,
                    ParentId = 0,
                    Title = "Về chúng tôi",
                    Name = "Về chúng tôi",
                    Action = "About",
                    Controller = "Home",
                    Slug = "/Home/About",
                    Params = "",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    IsActive = true,
                    IsHorizontal = true,
                },
                new Menu() {
                    Id = 4,
                    ParentId = 0,
                    Title = "Liên hệ",
                    Name = "Liên hệ",
                    Action = "Contact",
                    Controller = "Home",
                    Slug = "/Home/Contact",
                    Params = "",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    IsActive = true,
                    IsHorizontal = true,
                }
            };
        }

        private static List<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category() {
                    Id = 1,
                    Name = "Branding",
                    ParentId = 0,
                    IsActive = true,
                    Description = "Branding",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                },
                new Category() {
                    Id = 2,
                    Name = "Design",
                    ParentId = 0,
                    IsActive = true,
                    Description = "Design",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                },
                new Category() {
                    Id = 3,
                    Name = "Photo",
                    ParentId = 0,
                    IsActive = true,
                    Description = "Photo",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                },
                new Category() {
                    Id = 4,
                    Name = "Coffee",
                    ParentId = 0,
                    IsActive = true,
                    Description = "Coffee",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                }
            };
        }

        private static List<Project> GetProjects()
        {
            return new List<Project>
            {
                new Project() {
                    Id=1,
                    Seo = "Best web solution",
                    Keyword = "Interior Design",
                    CategoryClasses = "branding design photo coffee",
                    Name = "Chung cư Tecco Garden",
                    CategoryId = 1,
                    Description = "Chung cư Tecco Garden",
                    Title = "Project title 1",
                    ShortDesc = "Lorem ipsum dolor sit amet consec adipiscing nulla quis fermentum hendrerit nisi diam viverra.",
                    Created = DateTime.Parse("2023-03-27T07:54:08.417"),
                    IsActive = true,
                    Investor = "Tecco Group",
                    Address = "Tứ Hiệp Thanh Trì",
                    LandArea = 14470,
                    ConstructionArea = 6531,
                    YearOfCompletion = 2020,
                    Architect = "Ngô Quang Mạnh",
                    Picture = "assets/images/parallax1.jpg",
                    Picture1 = "assets/images/popup/small-1-1.jpg",
                    Picture2 = "assets/images/popup/small-2-1.jpg",
                    Picture3 = "assets/images/popup/small-3-1.jpg",
                    Picture4 = "assets/images/popup/small-4-1.jpg",
                    Intro = "Aenean suscipit eget mi act",
                    IntroContent = "<p>Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt.  Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt.</p><p>Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus   vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt.</p>",
                    Intro1 = "Aenean suscipit eget mi act",
                    Intro1Content = "<p>Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus.</p>",
                    Intro2 = "Aenean suscipit eget mi act",
                    Intro2Content = "<p>Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus.</p>",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                },
            };
        }
    }
}