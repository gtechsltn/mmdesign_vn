using Mmdesign.Models.Entity;
using System.Collections.Generic;

namespace Mmdesign.Models.Business
{
    public interface IProjectService
    {
        IEnumerable<Project> GetProjects();

        Project GetProject(int id);

        void CreateProject(Project project);

        void SaveProject();
    }
}