using Mmdesign.Infrastructure;
using System;
using System.Linq;

namespace Mmdesign.Models.Business
{
    public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Menu GetMenuByName(string menuName)
        {
            var menu = this.DbContext.Menus.Where(c => c.Name == menuName).FirstOrDefault();

            return menu;
        }

        public override void Update(Menu entity)
        {
            entity.DateUpdated = DateTime.Now;
            base.Update(entity);
        }
    }
}