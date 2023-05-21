using Mmdesign.Models;

namespace Mmdesign.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private MyContextDb dbContext;

        public MyContextDb Init()
        {
            return dbContext ?? (dbContext = new MyContextDb());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}