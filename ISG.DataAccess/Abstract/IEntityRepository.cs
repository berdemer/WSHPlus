using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace ISG.DataAccess.Abstract
{
    public interface IEntityRepository<T> where T : class , IEntity, new()
    {
        T Get (Expression<Func<T,bool>>filter);
        
        List<T> GetList(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>,IOrderedQueryable<T>> orderBy=null,
            int?page=null,int? pageSize=null);
        void Add(T entity);
        void Update(T entity, int key);
        void Delete(T entity);

        //Asenkron İşlemleri
        Task<T> GetAsync(int id);
        Task<T> GetAsync(Guid id);
        Task<ICollection<T>> GetAllAsync();

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> filter = null);

        Task<T> AddAsync(T entity);

        Task<IEnumerable<T>> AddAllAsync(IEnumerable<T> entityList);

        Task<T> UpdateAsync(T entity, int key);

        Task<T> UpdateAsync(T entity, Guid key);

        Task<T> UpdateStringAsync(T entity, string key);

        Task<int> DeleteAsync(long key);

        Task<int> DeleteAsync(int key);

        Task<int> DeleteAsync(Guid key);
        Task<int> CountAsync();
        Task<int> FullDeleteAsync();
    }
}
