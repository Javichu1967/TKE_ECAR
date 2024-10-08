using System.Data.Entity;
using System.Linq.Expressions;
using System;
using System.Linq;
using TK_ECAR.Domain.Specifications;
using System.Collections.Generic;

namespace TK_ECAR.Domain
{
    public partial interface IRepositoryBase<T> where T : class
    {
         T GetByID(object id); 
         void Insert(T entity);	 		 
		 void Delete(object id);
         void Delete(T entityToDelete);
		 void RemoveRange(IEnumerable<T> entity);
         void Update(T entityToUpdate);
         IQueryable<T> FindAll(Expression<Func<T, bool>> filter = null);
         IQueryable<T> FindAll(out int totalRows, Expression<Func<T, bool>> filter = null, int skip = 0, int take = 10);		  
         T FindOne(Expression<Func<T, bool>> filter);
         IQueryable<T> Fetch();   
		 IQueryable<T> Where(ISpecification<T> specfication);
		 IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
         DbContext InternalContext { get; }
        
    }
}