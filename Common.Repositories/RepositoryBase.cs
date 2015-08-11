using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Repositories
{

    public abstract class RepositoryBase : IDisposable
    {
        private DbContext _DataContext;


        public abstract DbContext GetDataContext();

        public virtual DbContext DataContext
        {
            get
            {
                if (_DataContext == null)
                {
                    _DataContext = GetDataContext();
                }
                return _DataContext;
            }
        }

        

        public void Dispose()
        {
            if (DataContext != null) DataContext.Dispose();
        }

    }
}