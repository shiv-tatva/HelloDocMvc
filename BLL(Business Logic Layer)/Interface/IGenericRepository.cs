﻿using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataModels;
using System.Linq.Expressions;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);

        Task<T> GetByIdAsync(int id);

        Task<T> GetByPropertyAsync(string property);


        public void Add(T model);
        public void Update(T model);

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter);

        public List<T> GetList(Expression<Func<T, bool>> predicate);

        public T IncludeEntity(Expression<Func<T, object>> select, Expression<Func<T, bool>> where);

        public void UpdateAndSaveChanges(Expression<Func<T, bool>> predicate, Action<T> updateAction);

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> filter);

        public void OnlyUpdate(T model);

        public void SaveChanges();

        public IEnumerable<T> GetAll();

        public int Count(Expression<Func<T, bool>> filter);

        public void Remove(T entity);

        public dynamic GetAllWithPagination(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int PageIndex, int PageSize, Expression<Func<T, object>> orderBy, bool IsAcc);

        public int GetTotalcount(Expression<Func<T, bool>> where);

        public dynamic SelectWhere(Expression<Func<T, object>> select, Expression<Func<T, bool>> where);

        public dynamic SelectWhereOrderBy(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy);

        public List<string> SelectWhere(Expression<Func<T, string>> select, Expression<Func<T, bool>> where);

    }
}
