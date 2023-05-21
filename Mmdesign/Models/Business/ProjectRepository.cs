using Mmdesign.Infrastructure;
using Mmdesign.Models.Entity;

namespace Mmdesign.Models.Business
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
}