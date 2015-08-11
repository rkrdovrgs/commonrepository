using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.DataAccess.Abstract
{
    public interface IDataContext
    {
        IQueryable<T> Get<T>(params Expression<Func<T, object>>[] path) where T : class;

        T Get<T>(int id, params Expression<Func<T, object>>[] path) where T : class, IEntity;

        T Add<T>(T entity) where T : class;

        void SaveChanges();

        void Delete<T>(int id) where T : class, IEntity;

        void Delete<T>(T entity) where T : class;
    }
}
