using Common.Core.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repositories.DataAccess
{
    public abstract class DataContext : IDataContext, IDisposable
    {
        private DbContext _dbContext;

        public abstract DbContext GetDbContext<T>();

        private DbContext DbContext<T>()
        {
            if (_dbContext == null)
                _dbContext = GetDbContext<T>();
            return _dbContext;
        }

        public IQueryable<T> Get<T>(params Expression<Func<T, object>>[] path) where T : class
        {
            IQueryable<T> query = DbContext<T>().Set<T>();
            return path.Aggregate(query, (current, child) => current.Include(child));
        }


        public T Get<T>(int id, params Expression<Func<T, object>>[] path) where T : class, IEntity
        {
            IQueryable<T> query = DbContext<T>().Set<T>().Where(x => x.Id == id);
            return path.Aggregate(query, (current, child) => current.Include(child)).FirstOrDefault();
        }


        public T Add<T>(T entity) where T : class
        {
            return DbContext<T>().Set<T>().Add(entity);
        }


        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }


        public void Delete<T>(int id) where T : class, IEntity
        {
            var objectSet = DbContext<T>().Set<T>().Where(x => x.Id == id);
            foreach (var item in objectSet)
            {
                DbContext<T>().Entry(item).State = EntityState.Deleted;
            }
        }


        public void Delete<T>(T entity) where T : class
        {
            DbContext<T>().Entry(entity).State = EntityState.Deleted;
        }

        public void Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}
