using System.Data.Entity;
using System.Linq.Expressions;
using System;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using System.Collections.Generic;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositoryBase<T> : IRepositoryBase<T> where T : class
	{
        private DbContext context;
        private DbSet<T> dbSet;

        public RepositoryBase(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public virtual T GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }


        public virtual void Delete(T entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }


        public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> filter = null)
        {
           
            var query = filter != null ? Fetch().Where(filter) : Fetch(); 

            return query;
        }

        public virtual IQueryable<T> FindAll(out int totalRows, Expression<Func<T, bool>> filter = null, int skip = 0, int take = 10)
        {
            IQueryable<T> query = FindAll(  filter );

            
            totalRows = query.Count();

            query = query.Skip(skip * take).Take(take);
  
            
            return query ;
        }


        public virtual T FindOne(Expression<Func<T, bool>> filter)
        {
           
            return Fetch().FirstOrDefault(filter);
        }

        public virtual IQueryable<T> Fetch()
        {
            IQueryable<T> query = dbSet;

            return query.AsQueryable();
        }

		public virtual IQueryable<T> Where(ISpecification<T> specfication)
        {
            IQueryable<T> query = dbSet;

            return query.Where(specfication.GetExpression()) ;
        }
		 
		public virtual IQueryable<T> Include(params Expression<Func<T, object>>[] includes)

        {
            IQueryable<T> query = dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public DbContext InternalContext
        {
            get { return this.context; }
        }
    }
}