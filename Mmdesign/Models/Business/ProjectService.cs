using Mmdesign.Infrastructure;
using Mmdesign.Models.Entity;
using System.Collections.Generic;

namespace Mmdesign.Models.Business
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectsRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProjectService(IProjectRepository projectsRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            this.projectsRepository = projectsRepository;
            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Project> GetProjects()
        {
            var projects = projectsRepository.GetAll();
            return projects;
        }

        public Project GetProject(int id)
        {
            var project = projectsRepository.GetById(id);
            return project;
        }

        public void CreateProject(Project project)
        {
            projectsRepository.Add(project);
        }

        public void SaveProject()
        {
            unitOfWork.Commit();
        }
    }
}