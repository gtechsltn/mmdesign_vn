using Mmdesign.Models.Entity;

namespace Mmdesign.Models.Business
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetCategoryByName(string categoryName);
    }
}