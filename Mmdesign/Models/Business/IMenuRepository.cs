namespace Mmdesign.Models.Business
{
    public interface IMenuRepository : IRepository<Menu>
    {
        Menu GetMenuByName(string menuName);
    }
}