using System.Collections.Generic;

namespace Mmdesign.Models
{
    public class ProjectViewModel
    {
        public ProjectModel Project { get; set; }
        public List<string> AllCategoryClasses { get; set; }
        public List<ProjectModel> TopListProjects { get; set; }
        public List<ProjectModel> RelatedProjects { get; set; }

        public ProjectViewModel()
        {
            Project = new ProjectModel();
            AllCategoryClasses = new List<string>()
            {
                "Branding",
                "Design",
                "Photo",
                "Coffee"
            };
            TopListProjects = new List<ProjectModel>();
            RelatedProjects = new List<ProjectModel>();
        }
    }
}