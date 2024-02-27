using ISG.DataAccess.Abstract;
using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class ,IEntity, new()
        where TContext : DbContext, new()
    {

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }


        public List<TEntity> GetList(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            int? page = null,
            int? pageSize = null
            )
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (orderBy != null)
                {
                    query = orderBy(query);
                }
                if (page != null & pageSize != null)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                return query.ToList();
            }

        }

        public ICollection<TEntity> FindAll(
           Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().
                    Where(filter).ToList();
            }
        }

        public void Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addEntity = context.Entry(entity);
                addEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }


        public void Update(TEntity entity, int key)
        {
            using (var context = new TContext())
            {
                TEntity existing = context.Set<TEntity>().Find(key);
                if (existing != null)
                {
                    context.Entry(existing).CurrentValues.SetValues(entity);
                    context.SaveChanges();
                }
            }
        }
        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addEntity = context.Entry(entity);
                addEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }


        //Asenkron İşlemleri
        public async Task<TEntity> GetAsync(int id)
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().FindAsync(id);
            }
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().FindAsync(id);
            }
        }
        public async Task<ICollection<TEntity>> GetAllAsync()
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().ToListAsync();
            }
        }


        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().SingleOrDefaultAsync(filter);
            }
        }



        public async Task<ICollection<TEntity>> FindAllAsync(
            Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().
                    Where(filter).ToListAsync();
            }
        }


        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
               context.Configuration.AutoDetectChangesEnabled = true;
                context.Set<TEntity>().Add(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }


        public async Task<IEnumerable<TEntity>> AddAllAsync(IEnumerable<TEntity> entityList)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().AddRange(entityList);
                await context.SaveChangesAsync();
                return entityList;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity updated, int key)
        {
            if (updated == null)
                return null;
            using (var context = new TContext())
            {
                TEntity existing = await context.Set<TEntity>().FindAsync(key);// güncellenecek modelin tümü alınır.
                if (existing != null)
                {
                    context.Configuration.ValidateOnSaveEnabled = false;

                    context.Entry(existing).CurrentValues.SetValues(updated);//güncellenecek modelin alanları değiştirilir ve güncellenir.

                    await context.SaveChangesAsync();
                }
                return existing ;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity updated, Guid key)
        {
            if (updated == null)
                return null;
            using (var context = new TContext())
            {
                TEntity existing = await context.Set<TEntity>().FindAsync(key);// güncellenecek modelin tümü alınır.
                if (existing != null)
                {
                    context.Configuration.ValidateOnSaveEnabled = false;

                    context.Entry(existing).CurrentValues.SetValues(updated);//güncellenecek modelin alanları değiştirilir ve güncellenir.

                    await context.SaveChangesAsync();
                }
                return existing;
            }
        }
        public async Task<TEntity> UpdateStringAsync(TEntity updated, string key)
        {
            if (updated == null)
                return null;
            using (var context = new TContext())
            {
                TEntity existing = await context.Set<TEntity>().FindAsync(key.Trim());// güncellenecek modelin tümü alınır.
                if (existing != null)
                {
                    context.Configuration.ValidateOnSaveEnabled = false;

                    context.Entry(existing).CurrentValues.SetValues(updated);//güncellenecek modelin alanları değiştirilir ve güncellenir.

                    await context.SaveChangesAsync();
                }
                return existing;
            }
        }
        public async Task<int> DeleteAsync(int key)
        {
            try
            {

                using (var context = new TContext())
                {
                    TEntity existing = await context.Set<TEntity>().FindAsync(key);

                    context.Set<TEntity>().Remove(existing);
                    return await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public async Task<int> DeleteAsync(long key)
        {
            using (var context = new TContext())
            {
                TEntity existing = await context.Set<TEntity>().FindAsync(key);

                context.Set<TEntity>().Remove(existing);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> DeleteAsync(Guid key)
        {
            using (var context = new TContext())
            {
                TEntity existing = await context.Set<TEntity>().FindAsync(key);

                context.Set<TEntity>().Remove(existing);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> CountAsync()
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().CountAsync();
            }
        }

        public async Task<int> FullDeleteAsync()
        {
            using (var context = new TContext())
            {
                List<TEntity> existing = await context.Set<TEntity>().ToListAsync();//Take(3).ToListAsync();
                context.Set<TEntity>().RemoveRange(existing);
                await context.SaveChangesAsync();

                //foreach (TEntity item in existing)
                //{
                //    context.Set<TEntity>().Remove(item);
                //    await context.SaveChangesAsync();
                //}
                return 1;
            }
        }

    }
}

