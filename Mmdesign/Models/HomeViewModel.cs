using System.Collections.Generic;

namespace Mmdesign.Models
{
    public class HomeViewModel
    {
        public List<ProjectModel> BestProjects { get; set; }

        public HomeViewModel()
        {
            BestProjects = new List<ProjectModel>();
        }
    }
}