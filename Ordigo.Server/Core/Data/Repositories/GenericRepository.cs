using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Ordigo.Server.Core.Contracts;

namespace Ordigo.Server.Core.Data.Repositories
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityBase
    {
        protected readonly DbContext mContext;

        protected DbSet<TEntity> Table => mContext.Set<TEntity>();

        protected GenericRepository(DbContext mContext)
        {
            this.mContext = mContext;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Table.AsNoTracking();
        }

        public virtual TEntity Find(Func<TEntity, bool> predicate)
        {
            return Table.FirstOrDefault(predicate);
        }

        public virtual TEntity GetById(int id)
        {
            return Table.FirstOrDefault(e => e.Id == id);
        }

        public bool Contains(int id)
        {
            return Table.Any(e => e.Id == id);
        }

        public bool Contains(Func<TEntity, bool> predicate)
        {
            return Table.Any(predicate);
        }

        public void Add(TEntity item)
        {
            Table.Add(item);
            mContext.SaveChanges();
        }

        public void Update(TEntity item)
        {
            if (!Contains(item.Id))
                throw new InvalidOperationException($"Item with ID {item.Id} doesn't exist!");

            Table.Update(item);
            mContext.SaveChanges();
        }

        public void Remove(TEntity item)
        {
            Table.Remove(item);
            mContext.SaveChanges();
        }

        public void Remove(int id)
        {
            var employee = Table.First(e => e.Id == id);

            Table.Remove(employee);
            mContext.SaveChanges();
        }
    }
}
