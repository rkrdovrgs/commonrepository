﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Common.Core.DataAccess;
using Common.Core.DataAccess.Abstract;


namespace Common.Core.Repositories
{
    public interface IRepository<T>
        where T : class, IEntity
    {
        T Get(int id);

        T Get(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetList(Expression<Func<T, bool>> predicate);
        /*
        IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy);

        IQueryable<T> GetList<TKey>(Expression<Func<T, TKey>> orderBy);
        */
        IQueryable<T> GetList();

        void Insert(T entity);

        void Update(T entity);

        void InsertOrUpdate(T entity);

        void InsertOrUpdateCollection(ICollection<T> entity);

        int ExecuteStoreCommand(string cmdText, params object[] parameters);

        void Delete(T entity);

        void DeleteCollection(ICollection<T> collection);

        void Delete(Expression<Func<T, bool>> predicate);

        //bool Save();

        //Pagination functions
        IQueryable<T> GetPageOrderByAscending<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy, int page, int pageSize);

        IQueryable<T> GetPageOrderByAscending<TKey>(Expression<Func<T, TKey>> orderBy, int page, int pageSize);

        IQueryable<T> GetPageOrderByDescendent<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy, int page, int pageSize);

        IQueryable<T> GetPageOrderByDescendent<TKey>(Expression<Func<T, TKey>> orderBy, int page, int pageSize);

    }
}
